using CommonLib.DTOs.EquipmentDTO;
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
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiServiceToDB.Data.EquipmentData
{
    public class EquipmentRepoMSSQL : BaseRepoMSSQL, IEquipmentRepo
    {
        public EquipmentRepoMSSQL(string connStr) : base(connStr) { }

        #region task-based

        public async Task<Result> EquipmentList(UserParams userParams, PagingParamsReq pagingParamsReq, EquipmentFilterParams equipmentFilterParams)
        {
            var task = Task.Run(() =>
            {
                return prcEquipmentList(userParams, pagingParamsReq, equipmentFilterParams);
            });

            Result taskResult = await task;
            return taskResult;
        }

        public async Task<Result> EquipmentSave(UserParams userParams, EquipmentToSaveDTO equipmentToSaveDTO)
        {
            var task = Task.Run(() =>
            {
                return prcEquipmentSave(userParams, equipmentToSaveDTO);
            });

            Result taskResult = await task;
            return taskResult;
        }

        #endregion task-based

        #region db-based

        private Result prcEquipmentList(UserParams userParams,
                                        PagingParamsReq pagingParamsReq,
                                        EquipmentFilterParams filterParams)
        {
            Result result;

            try
            {
                                           
                using (MedyanaDBContext medyanaDBContext = new MedyanaDBContext(_dbConn.ConnectionString))
                {

                    List<Equipment> filteredCollection = new List<Equipment>();

                    //------- step 1 -- build where,order,etc ----

                    List<WhereFilter> WhereFilter = new List<WhereFilter>();

                    WhereFilter.Add(new WhereFilter { Property = DataColumns.EQUIPMENT_ID, Value = 0, Operation = WhereOp.GreaterThanOrEqual });

                    if (filterParams.EquipmentId != 0)                        
                        WhereFilter.Add(new WhereFilter { Property = DataColumns.EQUIPMENT_ID, Value = filterParams.ClinicId, Operation = WhereOp.Equals });


                    if (filterParams.EquipmentName.Trim() != "")                        
                        WhereFilter.Add(new WhereFilter { Property = DataColumns.EQUIPMENT_NAME, Value = filterParams.EquipmentName.Trim(), Operation = WhereOp.Contains });



                    if (filterParams.ClinicId != 0)                        
                        WhereFilter.Add(new WhereFilter { Property = DataColumns.CLINIC_ID, Value = filterParams.ClinicId, Operation = WhereOp.Equals });

                    
                    Func<Equipment, bool> WhereExpr = WhereExpressionBuilder.GetExpression<Equipment>(WhereFilter).Compile();
                   
                    string orderColumnName = pagingParamsReq.OrderColumnName.Trim() != "" ? pagingParamsReq.OrderColumnName.Trim() : DataColumns.EQUIPMENT_ID;
                    Func<Equipment, object> OrderExpr = OrderExpressionBuilder.GetExpression<Equipment>(orderColumnName).Compile();

                    //------- /step 1 -- build where,order,etc ----


                    //------- step 2 --get data ----                    

                    OrderDirection orderDirection = pagingParamsReq.OrderAscDesc == "DESC" ? OrderDirection.DESC : OrderDirection.ASC;

                    if (orderDirection == OrderDirection.DESC)
                        filteredCollection = medyanaDBContext.Equipment.Where(WhereExpr).OrderByDescending(OrderExpr).Skip((pagingParamsReq.PageNumber - 1) * pagingParamsReq.PageRowCount).Take(pagingParamsReq.PageRowCount).ToList();
                    else
                        filteredCollection = medyanaDBContext.Equipment.Where(WhereExpr).OrderBy(OrderExpr).Skip((pagingParamsReq.PageNumber - 1) * pagingParamsReq.PageRowCount).Take(pagingParamsReq.PageRowCount).ToList();

                    //------- /step 2 --get data ----

                    result = EvaluateEquipmentResult(filteredCollection, ENUM_CRUD_TYPE.LIST);


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

        private Result prcEquipmentSave(UserParams userParams, EquipmentToSaveDTO equipmentToSaveDTO)
        {
            Result result;

            try
            {
                using (MedyanaDBContext medyanaDBContext = new MedyanaDBContext(_dbConn.ConnectionString))
                {

                    Equipment equipment = new Equipment()
                    {
                        EquipmentId = equipmentToSaveDTO.EquipmentId,
                        ClinicId = equipmentToSaveDTO.ClinicId,
                        EquipmentName = equipmentToSaveDTO.EquipmentName,
                        ProcurementDate = equipmentToSaveDTO.ProcurementDate,
                        UnitPrice = equipmentToSaveDTO.UnitPrice,
                        UsageRate = equipmentToSaveDTO.UsageRate,
                        Quantity = equipmentToSaveDTO.Quantity
                    };

                    ENUM_CRUD_TYPE eNUM_CRUD_TYPE = (equipment.EquipmentId == 0) ? ENUM_CRUD_TYPE.INSERT : ENUM_CRUD_TYPE.UPDATE;

                    if (equipment.EquipmentId == 0)
                        medyanaDBContext.Equipment.Add(equipment);
                    else
                        medyanaDBContext.Equipment.Update(equipment);


                    medyanaDBContext.SaveChanges();

                    result = EvaluateEquipmentResult(new List<Equipment> { equipment }, eNUM_CRUD_TYPE);
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

        private Result EvaluateEquipmentResult(List<Equipment> modeLList, ENUM_CRUD_TYPE eNUM_CRUD_TYPE)
        {
            Result result = new Result();
            List<EquipmentToListDTO> DTOlist = new List<EquipmentToListDTO>();

            //------            
            CRUDresult cRUDresult = (new CommonFuncLib()).GetCRUDresult(eNUM_CRUD_TYPE);
            //------

            if (modeLList != null && modeLList.Count > 0)
            {

                DTOlist = modeLList.AsEnumerable()
                                        .Select(Listitem => new EquipmentToListDTO
                                        {
                                            EquipmentId = Listitem.EquipmentId,
                                            ClinicId = Listitem.ClinicId,
                                            EquipmentName = Listitem.EquipmentName,
                                            //ClinicName = Listitem.ClinicName,
                                            Quantity = Listitem.Quantity,
                                            ProcurementDate = Listitem.ProcurementDate,
                                            UnitPrice = Listitem.UnitPrice,
                                            UsageRate = Listitem.UsageRate,

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
                        PkId = 0,
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
