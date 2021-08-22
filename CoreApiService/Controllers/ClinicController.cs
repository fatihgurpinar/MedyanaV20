using CommonLib.DTOs.ClinicDTO;
using CommonLib.DTOs.CommonDTO;
using CommonLib.Enums;
using CommonLib.Models.Misc;
using CommonLib.Params.FilterParams;
using CommonLib.Params.PagingParams;
using CommonLib.Params.QueryParams;
using CommonLib.Utilities;
using CoreApiServiceToDB.Data.ClinicData;
using CoreApiServiceToDB.Logger.LoggerToTxt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreApiService.Controllers
{
    /// <summary>
    /// Clinic API methods
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ClinicController : ControllerBase
    {
        private readonly IClinicRepo _repo;
        private readonly IConfiguration _config;
        private readonly ILoggerToTxt _logger;
        //private readonly IMapper _mapper;

        private CommonFuncLib _commonFuncLib;

       /// <summary>
       /// 
       /// </summary>
       /// <param name="repo"></param>
       /// <param name="config"></param>
       /// <param name="logger"></param>
        public ClinicController
         (
             IClinicRepo repo,
             IConfiguration config,
             ILoggerToTxt logger
             //IMapper mapper
         )
        {
            _repo = repo;
            _config = config;
            _logger = logger;
            //_mapper = mapper;

            _commonFuncLib = new CommonFuncLib();
        }

        #region clinic-CRUD

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTest")]
        public IEnumerable<string> GetTest()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clinicFilterParams"></param>
        /// <returns></returns>
        [HttpPost("ClinicList")]
        public async Task<IActionResult> ClinicList(ClinicFilterParams clinicFilterParams)
        {
            try
            {
                //get user info from JWT token
                UserParams userParams = new UserParams { UserID=1, LanguageCode="EN"};
                    //_commonFuncLib.GetUserParamsFromTokenStr(User.FindFirst(ClaimTypes.UserData).Value);

                //get data from repo
                Result result = await _repo.ClinicList(userParams, clinicFilterParams.PagingParamsReq, clinicFilterParams);

                //evaluate result
                if (result.ResultCode < 0)
                {
                    return Ok(new ResultDTO
                    {
                        ResultCode = result.ResultCode.ToString(),
                        ResultMessage = result.ResultMessage
                    });
                }
                else
                {
                    PagingParamsResp pagingParamsResp = new PagingParamsResp();
                    pagingParamsResp.pagingParamsReq = clinicFilterParams.PagingParamsReq;
                    pagingParamsResp.TotalRowCount = result.TotalRowCount;

                    //----------------- Added for ALL ---
                    if (clinicFilterParams.PagingParamsReq.PageRowCount == -1)
                    {
                        clinicFilterParams.PagingParamsReq.PageNumber = 1;
                        clinicFilterParams.PagingParamsReq.PageRowCount = result.TotalRowCount;
                    }
                    //----------------- /Added for ALL ---

                    CRUDresult cRUDresult = _logger.SaveLog(ENUM_LOG_TYPE.INFO, "clinic list called").GetAwaiter().GetResult();

                    return Ok(new ResultDTO
                    {
                        ResultCode = result.ResultCode.ToString(),
                        ResultMessage = result.ResultMessage,
                        ResultJSONobj = new
                        {
                            list = (List<ClinicToListDTO>)result.obj,
                            pagingParams = pagingParamsResp
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                string currentActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string currentControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


                CRUDresult cRUDresult = _logger.SaveLog(ENUM_LOG_TYPE.ERROR, ex.Message).GetAwaiter().GetResult();

                return Ok(new ResultDTO
                {
                                        
                    ResultCode =    Messages.EXCEPTION_CODE_IN_CONTROLLER.ToString(),
                    ResultMessage = Messages.EXCEPTION_MESSAGE_IN_CONTROLLER + " - " + ex.Message + "  " +
                                    currentControllerName +
                                    " - " +
                                    currentActionName
                });
            }

        }// ClinicList


        /// <summary>
        /// 
        /// </summary>
        /// <param name="clinicToSaveDTO"></param>
        /// <returns></returns>
        [HttpPost("ClinicSave")]
        public async Task<IActionResult> ClinicSave(ClinicToSaveDTO clinicToSaveDTO)
        {
            try
            {
                //get user info from JWT token
                UserParams userParams = new UserParams { UserID = 1, LanguageCode = "EN" };
                                        //_commonFuncLib.GetUserParamsFromTokenStr(User.FindFirst(ClaimTypes.UserData).Value);


                //get data from repo
                Result result = await _repo.ClinicSave(userParams, clinicToSaveDTO);

                //evaluate result
                if (result.ResultCode < 0)
                {
                    CRUDresult cRUDresult = _logger.SaveLog(ENUM_LOG_TYPE.ERROR, "error while saving clinic with id : " + result.PkId + " : " + result.ResultMessage).GetAwaiter().GetResult();

                    return Ok(new ResultDTO
                    {
                        ResultCode = result.ResultCode.ToString(),
                        ResultMessage = result.ResultMessage
                    });
                }
                else
                {
                    CRUDresult cRUDresult = _logger.SaveLog(ENUM_LOG_TYPE.INFO, "clinic saved with id : " + result.PkId ).GetAwaiter().GetResult();

                    return Ok(new ResultDTO
                    {
                        ResultCode = result.ResultCode.ToString(),
                        ResultMessage = result.ResultMessage,
                        ResultPkID = result.PkId.ToString(),
                        ResultJSONobj = new
                        {
                            list = (List<ClinicToListDTO>)result.obj,
                            totalRowCount = result.TotalRowCount
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                string currentActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string currentControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                CRUDresult cRUDresult = _logger.SaveLog(ENUM_LOG_TYPE.ERROR, ex.Message).GetAwaiter().GetResult();

                return Ok(new ResultDTO
                {
                    ResultCode = Messages.EXCEPTION_CODE_IN_CONTROLLER.ToString(),
                    ResultMessage = Messages.EXCEPTION_MESSAGE_IN_CONTROLLER + " - " + ex.Message + "  " +
                                    currentControllerName +
                                    " - " +
                                    currentActionName
                });
            }

        }//ClinicSave

        #endregion clinic-CRUD
    }
}
