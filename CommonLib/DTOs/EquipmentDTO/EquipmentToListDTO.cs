using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.DTOs.EquipmentDTO
{
    public class EquipmentToListDTO
    {
        public long EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public DateTime? ProcurementDate { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? UsageRate { get; set; }
        public int? ClinicId { get; set; }
        public int? Active { get; set; }
        public string ClinicName { get; set; }
        public string FormattedCreationDate { get; set; }
        public string CreatedByFullName { get; set; }
        public string FormattedLastUpdatedDate { get; set; }
        public string LastUpdatedByFullName { get; set; }
    }
}
