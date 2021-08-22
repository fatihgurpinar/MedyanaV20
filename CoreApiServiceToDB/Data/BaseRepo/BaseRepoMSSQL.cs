using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CoreApiServiceToDB.Data.BaseRepo
{
    public abstract class BaseRepoMSSQL : BaseRepoDB
    {
        protected readonly IDbConnection _dbConn;

        public BaseRepoMSSQL(string connStr) : base(connStr)
        {
            _dbConn = new SqlConnection(_connStr);
        }

    }

}
