using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace RSK.Audit.EF
{
    internal interface ICriteriaFactory
    {
        Expression<Func<T, bool>> CreateStringMatchExpression<T>(Matches matchType, string propertyToMatch,
            string match);
    }


    internal class ExpressionCriteriaFactory : ICriteriaFactory
    {
        private static Dictionary<Matches, MethodInfo> matchingFunctions = new Dictionary<Matches, MethodInfo>()
        {
            [Matches.Exactly] = typeof(string).GetMethod("Equals", new Type[] { typeof(string) }),
            [Matches.StartsWith] = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) })
        };

        public Expression<Func<T, bool>> CreateStringMatchExpression<T>(Matches matchType, string propertyToMatch, string match)
        {
            var matchStringExpression = Expression.Constant(match);
            ParameterExpression[] matchParams = new ParameterExpression[]
            {
                Expression.Parameter(typeof(AuditEntity), "ae")
            };

            MethodInfo equals = matchingFunctions[matchType];

            MethodInfo propGet = typeof(AuditEntity).GetProperty(propertyToMatch).GetMethod;

            Expression criteriaPropertyExpr = Expression.Property(matchParams[0], propGet);

            Expression criterExpression = Expression.Call(criteriaPropertyExpr, @equals, matchStringExpression);

            Expression<Func<T, bool>> criteria = Expression.Lambda<Func<T, bool>>(
                criterExpression,
                matchParams);

            return criteria;
        }
    }
}