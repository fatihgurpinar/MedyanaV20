using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.DTOs.CommonDTO
{
    public class ResultDTO
    {
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public string ResultPkID { get; set; }
        public Object ResultJSONobj { get; set; }

        public ResultDTO()
        {
            ResultCode = "-1";
            ResultMessage = "--";
            ResultPkID = "0";
        }
    }
}
