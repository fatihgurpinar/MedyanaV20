using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Params.PagingParams
{
    public class PagingParamsResp
    {
        public int TotalRowCount { get; set; }
        public PagingParamsReq pagingParamsReq { get; set; }
    }
}
