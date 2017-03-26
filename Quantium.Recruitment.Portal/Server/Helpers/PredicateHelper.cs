using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Quantium.Recruitment.Portal.Server.Helpers
{
    public static class PredicateHelper
    {
        public static Expression<Func<T, bool>> CombineWithAnd<T>(Expression<Func<T, bool>> firstExpression, Expression<Func<T, bool>> secondExpression)
        {
            // Create a parameter to use for both of the expression bodies.
            var parameter = Expression.Parameter(typeof(T), "x");
            // Invoke each expression with the new parameter, and combine the expression bodies with OR.
            var resultBody = Expression.And(Expression.Invoke(firstExpression, parameter), Expression.Invoke(secondExpression, parameter));
            // Combine the parameter with the resulting expression body to create a new lambda expression.
            return Expression.Lambda<Func<T, bool>>(resultBody, parameter);
        }
    }
}
