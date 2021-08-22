    using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CoreApiServiceToDB.Data.BaseRepo
{
    public abstract class BaseRepoMYSQL : BaseRepoDB
    {
        protected readonly IDbConnection _dbConn;

        public BaseRepoMYSQL(string connStr) : base(connStr)
        {
            // _dbConn = new MySqlConnection(_connStr);   // to use mysql add mysql library from nuget
        }
    }
}
