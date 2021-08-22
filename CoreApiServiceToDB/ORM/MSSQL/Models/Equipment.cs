using System;
using System.Collections.Generic;

#nullable disable

namespace CoreApiServiceToDB.ORM.MSSQL.Models
{
    public partial class Equipment
    {
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public DateTime? ProcurementDate { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? UsageRate { get; set; }
        public int? ClinicId { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedByUserId { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? Active { get; set; }

        public virtual Clinic Clinic { get; set; }
    }
}
