using System;
using System.Collections.Generic;
using System.Text;

namespace CoreApiServiceToDB.Data.BaseRepo
{
    public abstract class BaseRepoDB
    {
        protected readonly string _connStr;

        //public BaseRepoDB(string connStr, string secretKey)
        public BaseRepoDB(string connStr)
        {
            _connStr = connStr;
        }
    }

}
