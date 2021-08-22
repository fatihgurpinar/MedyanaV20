using CommonLib.Params.PagingParams;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Params.FilterParams
{
    public class EquipmentFilterParams
    {
        public EquipmentFilterParams()
        {
            EquipmentId = 0;
            EquipmentName = "";
            ClinicId = 0;

            PagingParamsReq = new PagingParamsReq();
        }

        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public DateTime ProcurementDate { get; set; }
        public int? ClinicId { get; set; }

        public PagingParamsReq PagingParamsReq { get; set; }
    }
}
