using CommonLib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreApiServiceToDB.Logger.LoggerToTxt
{
    public abstract class BaseLoggerToTxt
    {
        protected readonly string folderPath;
        protected readonly CommonFuncLib commonFuncLib;
        public BaseLoggerToTxt(string folderPath)
        {
            this.folderPath = folderPath;
            this.commonFuncLib = new CommonFuncLib();
        }
    }


}
