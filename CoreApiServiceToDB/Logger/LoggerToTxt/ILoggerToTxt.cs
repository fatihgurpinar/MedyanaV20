using CommonLib.Enums;
using CommonLib.Models.Misc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreApiServiceToDB.Logger.LoggerToTxt
{
    public interface ILoggerToTxt
    {
        //if it is required any log-to-txt scenario can be implemented here
        //ILoggerToTxt will be injected into desired code
        Task<CRUDresult> SaveLog(ENUM_LOG_TYPE eNUM_LOG_TYPE, string LogText);

    }


}
