using CommonLib.Enums;
using CommonLib.Models.Misc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreApiServiceToDB.Logger.LoggerToTxt
{
    public class LoggerToTxtLocalDisc : BaseLoggerToTxt, ILoggerToTxt
    {


        public LoggerToTxtLocalDisc(string folderPath) : base(folderPath) { }

        public async Task<CRUDresult> SaveLog(ENUM_LOG_TYPE eNUM_LOG_TYPE, string LogText)
        {


            var task = Task.Run(() =>
            {


                CRUDresult cRUDresult = new CRUDresult() { ResultCode = (int)(ENUM_RESULT_TYPE.OK), ResultMessage = "OK" };

                try
                {

                    string eNUM_LOG_TYPE_str = Enum.GetName(typeof(ENUM_LOG_TYPE), eNUM_LOG_TYPE);

                    string fileName = folderPath +
                                      "log-" +
                                      DateTime.Now.Year.ToString() + "-" +
                                      DateTime.Now.ToString("MM") + "-" +
                                      DateTime.Now.ToString("dd") + "-" +
                                      DateTime.Now.ToString("hh") +
                                      ".txt";

                    cRUDresult = commonFuncLib.WriteToFile(fileName,
                                                            DateTime.Now.ToString() + " : " + eNUM_LOG_TYPE_str + " : " + LogText +
                                                            Environment.NewLine);

                }
                catch (Exception ex)
                {
                    cRUDresult = new CRUDresult() { ResultCode = (int)(ENUM_RESULT_TYPE.ERROR), ResultMessage = "err:" + ex.Message };
                }

                return cRUDresult;

            });

            CRUDresult taskResult = await task;
            return taskResult;

        }

    }


}
