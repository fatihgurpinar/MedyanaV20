﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CommonLib.Pagination.WhereBuilder
{
    public static class WhereExpressionBuilder
    {
       
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string), typeof(StringComparison) });       
        private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string), typeof(StringComparison) });       
        private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string), typeof(StringComparison) });       
        private static MethodInfo stringEqualsMethod = typeof(string).GetMethod("Equals", new Type[] { typeof(string), typeof(StringComparison) });
       

        public static Expression<Func<T, bool>> GetExpression<T>(IList<WhereFilter> filters)
        {
            if (filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            if (filters.Count == 1)
                exp = GetExpression<T>(param, filters[0]);
            else if (filters.Count == 2)
                exp = GetExpression<T>(param, filters[0], filters[1]);
            else
            {
                while (filters.Count > 0)
                {
                    var f1 = filters[0];
                    var f2 = filters[1];

                    if (exp == null)
                        exp = GetExpression<T>(param, filters[0], filters[1]);
                    else
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0], filters[1]));

                    filters.Remove(f1);
                    filters.Remove(f2);

                    if (filters.Count == 1)
                    {
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        private static Expression GetExpression<T>(ParameterExpression param, WhereFilter filter)
        {

            MemberExpression member = Expression.Property(param, filter.Property);
            ConstantExpression constant = Expression.Constant(filter.Value);

            //---- case insensitivity --            
            ConstantExpression stringComparisonCultureConstant = Expression.Constant(StringComparison.CurrentCultureIgnoreCase);            
            //---- /case insensitivity --

            switch (filter.Operation)
            {
                case WhereOp.Equals:
                    return Expression.Equal(member, constant);

                case WhereOp.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case WhereOp.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case WhereOp.LessThan:
                    return Expression.LessThan(member, constant);

                case WhereOp.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case WhereOp.Contains:
                    return Expression.Call(member, containsMethod, constant, stringComparisonCultureConstant);
               
                case WhereOp.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant, stringComparisonCultureConstant);

                case WhereOp.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant, stringComparisonCultureConstant);
             
                case WhereOp.StringEquals:             
                    return Expression.Call(member, stringEqualsMethod, constant, stringComparisonCultureConstant);
                    
            }

            return null;
        }

        private static BinaryExpression GetExpression<T>
        (ParameterExpression param, WhereFilter filter1, WhereFilter filter2)
        {
            Expression bin1 = GetExpression<T>(param, filter1);
            Expression bin2 = GetExpression<T>(param, filter2);

            return Expression.AndAlso(bin1, bin2);
        }

    }


}
