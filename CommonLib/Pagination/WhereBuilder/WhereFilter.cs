using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Pagination.WhereBuilder
{
    public class WhereFilter
    {
        public string Property { get; set; }
        public object Value { get; set; }

        public WhereOp Operation { get; set; }

    }

}
