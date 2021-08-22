using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Pagination.WhereBuilder
{
    public enum WhereOp
    {
        Equals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        StartsWith,
        EndsWith,
        StringEquals
    }

}
