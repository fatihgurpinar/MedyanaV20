using CommonLib.Enums;
using CommonLib.Models.Misc;
using CommonLib.Params.QueryParams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonLib.Utilities
{
    public class CommonFuncLib
    {
        /// <summary>
        /// get user info
        /// </summary>
        /// <param name="TokenStr"></param>
        /// <returns></returns>
        public UserParams GetUserParamsFromTokenStr(String TokenStr)
        {
            //UserData : UserId,LanguageCode
            string UserData = TokenStr; 
            string[] UserParamsItemList = UserData.Split(",");
            UserParams userParams = new UserParams()
            {
                UserID = Convert.ToInt32(UserParamsItemList[0]),
                LanguageCode = UserParamsItemList[0]
            };
            return userParams;
        }

        public CRUDresult GetCRUDresult(ENUM_CRUD_TYPE eNUM_CRUD_TYPE) {

            CRUDresult cRUDresult = new CRUDresult();

            if (eNUM_CRUD_TYPE == ENUM_CRUD_TYPE.LIST)
            {
                cRUDresult.ResultCode = Messages.OK_LISTED_CODE;
                cRUDresult.ResultMessage = Messages.OK_LISTED_MESSAGE;
            }
            else if (eNUM_CRUD_TYPE == ENUM_CRUD_TYPE.INSERT)
            {
                cRUDresult.ResultCode = Messages.OK_ADDED_CODE;
                cRUDresult.ResultMessage = Messages.OK_ADDED_MESSAGE;
            }
            else if (eNUM_CRUD_TYPE == ENUM_CRUD_TYPE.UPDATE)
            {
                cRUDresult.ResultCode = Messages.OK_UPDATED_CODE;
                cRUDresult.ResultMessage = Messages.OK_UPDATED_MESSAGE;
            }
            return cRUDresult;
        }



        public CRUDresult WriteToFile(string fileName, string txt)
        {
            CRUDresult cRUDresult = new CRUDresult() { ResultCode = (int)(ENUM_RESULT_TYPE.OK), ResultMessage = "OK" };

            try
            {
                using (StreamWriter sw = new StreamWriter(File.Open(fileName, FileMode.Append)))
                {
                    sw.Write(txt);
                }

            }
            catch (Exception ex)
            {
                cRUDresult = new CRUDresult() { ResultCode = (int)(ENUM_RESULT_TYPE.ERROR), ResultMessage = "err:" + ex.Message };
            }

            return cRUDresult;
        }

    }
}
