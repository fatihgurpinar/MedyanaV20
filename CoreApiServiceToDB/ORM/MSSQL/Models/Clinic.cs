using System;
using System.Collections.Generic;

#nullable disable

namespace CoreApiServiceToDB.ORM.MSSQL.Models
{
    public partial class Clinic
    {
        public Clinic()
        {
            Equipment = new HashSet<Equipment>();
        }

        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedByUserId { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? Active { get; set; }

        public virtual ICollection<Equipment> Equipment { get; set; }
    }
}
