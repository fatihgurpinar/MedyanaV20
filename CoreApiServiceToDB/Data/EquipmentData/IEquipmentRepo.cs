using CommonLib.DTOs.EquipmentDTO;
using CommonLib.Models.Misc;
using CommonLib.Params.FilterParams;
using CommonLib.Params.PagingParams;
using CommonLib.Params.QueryParams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreApiServiceToDB.Data.EquipmentData
{
    public interface IEquipmentRepo
    {
        #region equipment

        Task<Result> EquipmentList(UserParams userParams, PagingParamsReq pagingParamsReq, EquipmentFilterParams equipmentFilterParams);

        Task<Result> EquipmentSave(UserParams userParams, EquipmentToSaveDTO equipmentToSaveDTO);

        #endregion equipment
    }
}
