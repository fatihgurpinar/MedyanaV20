using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Models.Misc
{
    public class Result
    {
        public Int64 PkId; //primary key

        public int ResultCode;
        public string ResultMessage;

        public int TotalRowCount;
        public int PageRowCount;

        public object obj;

        public Result()
        {
            ResultCode = -101;
            ResultMessage = "";
            PkId = 0;
            TotalRowCount = 0;
            PageRowCount = 0;

            obj = null;
        }
    }
}
