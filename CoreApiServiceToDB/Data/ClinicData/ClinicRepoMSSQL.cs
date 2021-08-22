using CommonLib.DTOs.ClinicDTO;
using CommonLib.Enums;
using CommonLib.Models.Misc;
using CommonLib.Pagination.OrderBuilder;
using CommonLib.Pagination.WhereBuilder;
using CommonLib.Params.FilterParams;
using CommonLib.Params.PagingParams;
using CommonLib.Params.QueryParams;
using CommonLib.Utilities;
using CoreApiServiceToDB.Data.BaseRepo;
using CoreApiServiceToDB.ORM.MSSQL.Context;
using CoreApiServiceToDB.ORM.MSSQL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiServiceToDB.Data.ClinicData
{
    public class ClinicRepoMSSQL: BaseRepoMSSQL, IClinicRepo
    {
        public ClinicRepoMSSQL(string connStr) : base(connStr) { }

        #region task-based

        public async Task<Result> ClinicList(UserParams userParams, PagingParamsReq pagingParamsReq, ClinicFilterParams clinicFilterParams)
        {
            var task = Task.Run(() =>
            {
                return prcClinicList(userParams, pagingParamsReq, clinicFilterParams);
            });

            Result taskResult = await task;
            return taskResult;
        }

        public async Task<Result> ClinicSave(UserParams userParams, ClinicToSaveDTO clinicToSaveDTO)
        {
            var task = Task.Run(() =>
            {
                return prcClinicSave(userParams, clinicToSaveDTO);
            });

            Result taskResult = await task;
            return taskResult;
        }

        #endregion task-based

        #region db-based
        
        private Result prcClinicList(   UserParams userParams, 
                                        PagingParamsReq pagingParamsReq, 
                                        ClinicFilterParams filterParams)
        {          
            Result result;

            
            //Type T2 = (new Clinic()).GetType();
            //Type T = typeof(Clinic);

            try
            {
                                         

                using (MedyanaDBContext medyanaDBContext = new MedyanaDBContext(_dbConn.ConnectionString)) 
                {


                    //List<Clinic> modelLList = medyanaDBContext.Clinics.ToList();


                    List<Clinic> filteredCollection = new List<Clinic>();

                    //------- step 1 -- build where,order,etc ----

                    List<WhereFilter> WhereFilter = new List<WhereFilter>();

                    WhereFilter.Add(new WhereFilter { Property = DataColumns.CLINIC_ID, Value = 0, Operation = WhereOp.GreaterThanOrEqual });

                    if (filterParams.ClinicId != 0)                        
                        WhereFilter.Add(new WhereFilter { Property = DataColumns.CLINIC_ID, Value = filterParams.ClinicId, Operation = WhereOp.Equals });


                    if (filterParams.ClinicName.Trim() != "")                        
                        WhereFilter.Add(new WhereFilter { Property = DataColumns.CLINIC_NAME, Value = filterParams.ClinicName.Trim(), Operation = WhereOp.Contains });

                    
                    Func<Clinic, bool> WhereExpr = WhereExpressionBuilder.GetExpression<Clinic>(WhereFilter).Compile();
                                       
                    string orderColumnName = pagingParamsReq.OrderColumnName.Trim() != "" ? pagingParamsReq.OrderColumnName.Trim() : DataColumns.CLINIC_ID;                                        
                    Func<Clinic, object> OrderExpr = OrderExpressionBuilder.GetExpression<Clinic>(orderColumnName).Compile();


                    //------- /step 1 -- build where,order,etc ----


                    //------- step 2 --get data ----
                    
                    OrderDirection orderDirection = pagingParamsReq.OrderAscDesc == "DESC" ? OrderDirection.DESC : OrderDirection.ASC;

                    if (orderDirection == OrderDirection.DESC)
                        filteredCollection = medyanaDBContext.Clinics.Where(WhereExpr).OrderByDescending(OrderExpr).Skip((pagingParamsReq.PageNumber - 1) * pagingParamsReq.PageRowCount).Take(pagingParamsReq.PageRowCount).ToList();                        
                    else
                        filteredCollection = medyanaDBContext.Clinics.Where(WhereExpr).OrderBy(OrderExpr).Skip((pagingParamsReq.PageNumber - 1) * pagingParamsReq.PageRowCount).Take(pagingParamsReq.PageRowCount).ToList();

                    //------- /step 2 --get data ----
                    
                    result = EvaluateClinicResult(filteredCollection, ENUM_CRUD_TYPE.LIST);

                }
            }
            catch (Exception ex)
            {

                result = new Result()
                {
                    ResultCode = Messages.ERROR_CODE,
                    ResultMessage = Messages.ERROR_MESSAGE + " : " + ex.Message
                };
            }

            return result;
        }

        private Result prcClinicSave(UserParams userParams, ClinicToSaveDTO clinicToSaveDTO)
        {
            Result result;

            try
            {
                using (MedyanaDBContext medyanaDBContext = new MedyanaDBContext(_dbConn.ConnectionString))
                {

                    Clinic clinic = new Clinic()
                    {
                        ClinicId = clinicToSaveDTO.ClinicId,
                        ClinicName = clinicToSaveDTO.ClinicName

                    };

                    ENUM_CRUD_TYPE eNUM_CRUD_TYPE = (clinic.ClinicId == 0) ? ENUM_CRUD_TYPE.INSERT : ENUM_CRUD_TYPE.UPDATE;

                    if (clinic.ClinicId == 0)                                        
                        medyanaDBContext.Clinics.Add(clinic);                                          
                    else                   
                        medyanaDBContext.Clinics.Update(clinic);
                    

                    medyanaDBContext.SaveChanges();

                    result = EvaluateClinicResult(new List<Clinic> { clinic }, eNUM_CRUD_TYPE);
                }
            }
            catch (Exception ex)
            {             
                result = new Result()
                {
                    ResultCode = Messages.ERROR_CODE,
                    ResultMessage = Messages.ERROR_MESSAGE + " : " + ex.Message
                };
            }
            return result;
        }

        #endregion db-based

        #region eval-based

        private Result EvaluateClinicResult(List<Clinic> modeLList, ENUM_CRUD_TYPE eNUM_CRUD_TYPE)
        {
            Result result = new Result();
            List<ClinicToListDTO> DTOlist = new List<ClinicToListDTO>();                     
            CRUDresult cRUDresult = (new CommonFuncLib()).GetCRUDresult(eNUM_CRUD_TYPE);
            

            if (modeLList!=null && modeLList.Count > 0)
            {

                DTOlist = modeLList.AsEnumerable()
                                        .Select(Listitem => new ClinicToListDTO
                                        {
                                            ClinicId = Listitem.ClinicId,
                                            ClinicName = Listitem.ClinicName,
                                            
                                            //...

                                            FormattedCreationDate = Listitem.CreationDate.HasValue ? Listitem.CreationDate.Value.ToString() : "",
                                            CreatedByFullName = "test user for creation",
                                            FormattedLastUpdatedDate = Listitem.LastUpdatedDate.HasValue ? Listitem.LastUpdatedDate.Value.ToString() : "",
                                            LastUpdatedByFullName = "test user for upd"

                                            //...

                                        }).ToList();


                if (DTOlist != null && DTOlist.Count > 0)
                {
                    result = new Result()
                    {
                        ResultCode = cRUDresult.ResultCode,
                        ResultMessage = cRUDresult.ResultMessage,
                        TotalRowCount = DTOlist.Count,
                        PkId =0,
                        obj = DTOlist
                    };
                }
                else
                {
                    result = new Result()
                    {
                        ResultCode = Messages.NOT_FOUND_ERROR_CODE,
                        ResultMessage = Messages.NOT_FOUND_ERROR_MESSAGE,
                        TotalRowCount = 0,
                        PkId = 0
                    };
                }
            }
            return result;
        }

        #endregion eval-based
    }
}
