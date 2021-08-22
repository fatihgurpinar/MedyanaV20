using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CommonLib.Pagination.OrderBuilder
{
    public static class OrderExpressionBuilder
    {

        //makes expression for specific prop
        public static Expression<Func<TSource, object>> GetExpression<TSource>(string propertyName)
        {
            var param = Expression.Parameter(typeof(TSource), "x");
            Expression conversion = Expression.Convert(Expression.Property(param, propertyName), typeof(object));   //important to use the Expression.Convert 
            return Expression.Lambda<Func<TSource, object>>(conversion, param);
        }

    }
}
