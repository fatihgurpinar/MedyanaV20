using System;
using System.Collections.Generic;
using System.Text;

namespace CoreApiServiceToDB.Data.BaseRepo
{
    
    public abstract class BaseRepoMONGO : BaseRepoDB
    {
        //mongo related conn object can be here!        


        public BaseRepoMONGO(string connStr) : base(connStr)
        {
            //...
        }


    }
}
