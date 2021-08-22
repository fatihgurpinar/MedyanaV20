using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLib.DTOs.CommonDTO;
using CommonLib.DTOs.EquipmentDTO;
using CommonLib.Enums;
using CommonLib.Models.Misc;
using CommonLib.Params.FilterParams;
using CommonLib.Params.PagingParams;
using CommonLib.Params.QueryParams;
using CommonLib.Utilities;
using CoreApiServiceToDB.Data.EquipmentData;
using CoreApiServiceToDB.Logger.LoggerToTxt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoreApiService.Controllers
{
    /// <summary>
    /// Equipment API methods
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentRepo _repo;
        private readonly IConfiguration _config;
        private readonly ILoggerToTxt _logger;
        //private readonly IMapper _mapper;

        private CommonFuncLib _commonFuncLib;

        /// <summary>
        /// This controller is used for equipment operations. It includes EquipmentList and EquipmentSave methods
        /// </summary>
        /// <param name="repo">This is injected repo parameter with pre-parameterized in startup</param>
        /// <param name="config">This is injected config parameter</param>
        /// <param name="logger"></param>     
        public EquipmentController
         (
             IEquipmentRepo repo,
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

        #region equipment-CRUD

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
        /// <param name="equipmentFilterParams"></param>
        /// <returns></returns>
        [HttpPost("EquipmentList")]
        public async Task<IActionResult> EquipmentList(EquipmentFilterParams equipmentFilterParams)
        {
            try
            {
                //get user info from JWT token
                UserParams userParams = new UserParams { UserID = 1, LanguageCode = "EN" };
                //_commonFuncLib.GetUserParamsFromTokenStr(User.FindFirst(ClaimTypes.UserData).Value);

                //get data from repo
                Result result = await _repo.EquipmentList(userParams, equipmentFilterParams.PagingParamsReq, equipmentFilterParams);

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
                    pagingParamsResp.pagingParamsReq = equipmentFilterParams.PagingParamsReq;
                    pagingParamsResp.TotalRowCount = result.TotalRowCount;

                    //----------------- Added for ALL ---
                    if (equipmentFilterParams.PagingParamsReq.PageRowCount == -1)
                    {
                        equipmentFilterParams.PagingParamsReq.PageNumber = 1;
                        equipmentFilterParams.PagingParamsReq.PageRowCount = result.TotalRowCount;
                    }
                    //----------------- /Added for ALL ---

                    CRUDresult cRUDresult = _logger.SaveLog(ENUM_LOG_TYPE.INFO, "equipment list called").GetAwaiter().GetResult();

                    return Ok(new ResultDTO
                    {
                        ResultCode = result.ResultCode.ToString(),
                        ResultMessage = result.ResultMessage,
                        ResultJSONobj = new
                        {
                            list = (List<EquipmentToListDTO>)result.obj,
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

                    ResultCode = Messages.EXCEPTION_CODE_IN_CONTROLLER.ToString(),
                    ResultMessage = Messages.EXCEPTION_MESSAGE_IN_CONTROLLER + " - " + ex.Message + "  " +
                                    currentControllerName +
                                    " - " +
                                    currentActionName
                });
            }

        }// EquipmentList


        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentToSaveDTO"></param>
        /// <returns></returns>
        [HttpPost("EquipmentSave")]
        public async Task<IActionResult> EquipmentSave(EquipmentToSaveDTO equipmentToSaveDTO)
        {
            try
            {
                //get user info from JWT token
                UserParams userParams = new UserParams { UserID = 1, LanguageCode = "EN" };
                //_commonFuncLib.GetUserParamsFromTokenStr(User.FindFirst(ClaimTypes.UserData).Value);


                //get data from repo
                Result result = await _repo.EquipmentSave(userParams, equipmentToSaveDTO);

                //evaluate result
                if (result.ResultCode < 0)
                {
                    CRUDresult cRUDresult = _logger.SaveLog(ENUM_LOG_TYPE.ERROR, "error while saving equipment with id : " + result.PkId + " : " + result.ResultMessage).GetAwaiter().GetResult();

                    return Ok(new ResultDTO
                    {
                        ResultCode = result.ResultCode.ToString(),
                        ResultMessage = result.ResultMessage
                    });
                }
                else
                {
                    CRUDresult cRUDresult = _logger.SaveLog(ENUM_LOG_TYPE.INFO, "equipment saved with id : " + result.PkId).GetAwaiter().GetResult();

                    return Ok(new ResultDTO
                    {
                        ResultCode = result.ResultCode.ToString(),
                        ResultMessage = result.ResultMessage,
                        ResultPkID = result.PkId.ToString(),
                        ResultJSONobj = new
                        {
                            list = (List<EquipmentToListDTO>)result.obj,
                            totalRowCount = result.TotalRowCount
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                string currentActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string currentControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                return Ok(new ResultDTO
                {
                    ResultCode = Messages.EXCEPTION_CODE_IN_CONTROLLER.ToString(),
                    ResultMessage = Messages.EXCEPTION_MESSAGE_IN_CONTROLLER + " - " + ex.Message + "  " +
                                    currentControllerName +
                                    " - " +
                                    currentActionName
                });
            }

        }//EquipmentSave

        #endregion equipment-CRUD
    }
}
