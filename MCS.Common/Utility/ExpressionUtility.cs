using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MCS.Common
{
    public class ExpressionUtility
    {
        public static Expression<Func<T, bool>> AndAlso<T>(Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            ReplaceExpressionVisitor leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            Expression left = leftVisitor.Visit(expr1.Body);
            ReplaceExpressionVisitor rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            Expression right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }

        public static Func<T, bool> CombineWithOr<T>(IEnumerable<Func<T, bool>> filters)
        {
            return x =>
            {
                foreach (var filter in filters)
                {
                    if (filter(x))
                    {
                        return true;
                    }
                }
                return false;
            };
        }

        public static Expression<Func<T, bool>> CombineWithOr<T>(Expression<Func<T, bool>> firstExp, Expression<Func<T, bool>> secondExp)
        {           
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            BinaryExpression resultBody = Expression.Or(Expression.Invoke(firstExp, parameter), Expression.Invoke(secondExp, parameter));
                       
            return Expression.Lambda<Func<T, bool>>(resultBody, parameter);
        }

        public static Func<T, bool> CombineWithAnd<T>(IEnumerable<Func<T, bool>> filters)
        {
            return x =>
            {
                foreach (var filter in filters)
                {
                    if (!filter(x))
                    {
                        return false;
                    }
                }

                return true;
            };
        }
    }
}
