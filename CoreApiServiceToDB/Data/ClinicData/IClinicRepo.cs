using CommonLib.DTOs.ClinicDTO;
using CommonLib.Models.Misc;
using CommonLib.Params.FilterParams;
using CommonLib.Params.PagingParams;
using CommonLib.Params.QueryParams;
using System.Threading.Tasks;

namespace CoreApiServiceToDB.Data.ClinicData
{
    public  interface IClinicRepo
    {
        #region clinic

        Task<Result> ClinicList(UserParams userParams, PagingParamsReq pagingParamsReq, ClinicFilterParams clinicFilterParams);

        Task<Result> ClinicSave(UserParams userParams, ClinicToSaveDTO clinicToSaveDTO);

        #endregion clinic
    }
}
