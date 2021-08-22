using CommonLib.Params.PagingParams;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Params.FilterParams
{
    public class ClinicFilterParams
    {
        public ClinicFilterParams()
        {
            ClinicId = 0;
            ClinicName = "";

            PagingParamsReq = new PagingParamsReq();
        }

        public int ClinicId { get; set; }
        public string ClinicName { get; set; }

        public PagingParamsReq PagingParamsReq { get; set; }
    }
}
