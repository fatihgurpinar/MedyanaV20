using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.DTOs.EquipmentDTO
{
    public class EquipmentToSaveDTO
    {
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public DateTime? ProcurementDate { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? UsageRate { get; set; }
        public int? ClinicId { get; set; }
    }
}
