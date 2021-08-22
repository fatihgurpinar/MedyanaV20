using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.DTOs.ClinicDTO
{
    public class ClinicToListDTO
    {
        public long ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string FormattedCreationDate { get; set; }
        public string CreatedByFullName { get; set; }
        public string FormattedLastUpdatedDate { get; set; }
        public string LastUpdatedByFullName { get; set; }
    }
}
