using CommonLib.Enums;
using CommonLib.Models.Misc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreApiServiceToDB.Logger.LoggerToTxt
{
    public class LoggerToTxtExternalDisc : BaseLoggerToTxt, ILoggerToTxt
    {

        public LoggerToTxtExternalDisc(string folderPath) : base(folderPath) { }

        public async Task<CRUDresult> SaveLog(ENUM_LOG_TYPE eNUM_LOG_TYPE, string LogText)
        {
            throw new NotImplementedException();
        }
    }

}
