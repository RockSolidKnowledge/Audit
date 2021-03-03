using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace RSK.Audit.EF
{

    public interface ICriteriaBuilder<T>
    {
        ICriteriaBuilder<T> AddStringTransform(string propertyName, Func<string, string> transform);

        ICriteriaBuilder<T> AndStringMatchAny(Matches matchType, string[] propertiesToMatch, string match);

        ICriteriaBuilder<T> AndStringMatch(Matches matchType, string propertyToMatch, string match);
        ICriteriaBuilder<T> OrStringMatch(Matches matchType, string propertyToMatch, string match);

        ICriteriaBuilder<T> AndBoolean(string propertyToMatch, bool match);
        ICriteriaBuilder<T> OrBoolean(string propertyToMatch, bool match);

        Expression<Func<T, bool>> Build();
    }

    internal class ExpressionCriteriaBuilder<T> : ICriteriaBuilder<T>
    {
        private static readonly Dictionary<Matches, MethodInfo> matchingFunctions = new Dictionary<Matches, MethodInfo>()
        {
            [Matches.Exactly] = typeof(string).GetMethod("Equals", new Type[] { typeof(string) }),
            [Matches.StartsWith] = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }),
            [Matches.EndsWith] = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }),
            [Matches.SomethingLike] = typeof(string).GetMethod("Contains", new Type[] { typeof(string) })

        };

        private readonly ParameterExpression[] matchParams = new ParameterExpression[]
        {
            Expression.Parameter(typeof(T), "ae")
        };

        public ExpressionCriteriaBuilder()
        {
              
        }

        private Expression CreateStringMatchExpression(Matches matchType, string propertyToMatch, string match)
        {
            if (inputTransformMap.TryGetValue(propertyToMatch, out Func<string, string> transform))
            {
                match = transform(match);
            }

            var matchStringExpression = Expression.Constant(match);
           

            MethodInfo equals = matchingFunctions[matchType];

            MethodInfo propGet = typeof(T).GetProperty(propertyToMatch).GetMethod;

            Expression criteriaPropertyExpr = Expression.Property(matchParams[0], propGet);

            Expression criteriaExpression = Expression.Call(criteriaPropertyExpr, @equals, matchStringExpression);

            return criteriaExpression;
        }

        private Expression CreateBooleanMatchExpression(string propertyToMatch, bool match)
        {

            var matchBooleanValue = Expression.Constant(match);


            

            MethodInfo propGet = typeof(T).GetProperty(propertyToMatch).GetMethod;

            Expression criteriaPropertyExpr = Expression.Property(matchParams[0], propGet);

            Expression criteriaExpression = Expression.Equal(criteriaPropertyExpr, matchBooleanValue);

            return criteriaExpression;
        }

        private Expression expression = null;

        private Stack<Expression> expressionStack = new Stack<Expression>();
     
        public ICriteriaBuilder<T> AndStringMatch(Matches matchType, string propertyToMatch, string match)
        {
           
            CombineStringMatch(Expression.And,matchType,propertyToMatch,match);
            return this;
        }


        public ICriteriaBuilder<T> AndStringMatchAny(Matches matchType, string[] propertiesToMatch, string match)
        {
            Expression subExpression = null;
            foreach (string propertyToMatch in propertiesToMatch)
            {
                var stringMatchExpression = CreateStringMatchExpression(matchType, propertyToMatch, match);

                subExpression = subExpression == null
                    ? stringMatchExpression
                    : Expression.Or(subExpression, stringMatchExpression);
            }

            expression = expression == null ? subExpression : Expression.And(expression, subExpression);

            return this;
        }

        public ICriteriaBuilder<T> OrStringMatch(Matches matchType, string propertyToMatch, string match)
        {
            CombineStringMatch(Expression.Or, matchType, propertyToMatch, match);

            return this;
        }

        public ICriteriaBuilder<T> AndBoolean(string propertyToMatch, bool match)
        {
            CombineBooleanMatch(Expression.And,propertyToMatch,match);

            return this;
        }

        public ICriteriaBuilder<T> OrBoolean(string propertyToMatch, bool match)
        {
            CombineBooleanMatch(Expression.Or, propertyToMatch, match);

            return this;
        }


        private void CombineStringMatch(Func<Expression, Expression, Expression> combine, Matches matchType,
            string propertyToMatch, string match)
        {
           

            var stringMatchExpression = CreateStringMatchExpression(matchType, propertyToMatch, match);

            expression = expression == null ? stringMatchExpression : combine(expression, stringMatchExpression);

        }

        private void CombineBooleanMatch(Func<Expression, Expression, Expression> combine, string propertyToMatch, bool match)
        {
            var boolMatchExpression = CreateBooleanMatchExpression( propertyToMatch, match);

            expression = expression == null ? boolMatchExpression : combine(expression, boolMatchExpression);

        }


        public Expression<Func<T, bool>> Build()
        {
            Expression<Func<T, bool>> criteria = expression != null
                ? Expression.Lambda<Func<T, bool>>(
                    expression,
                    matchParams)
                : _ => true;

            return criteria;
        }

        Dictionary<string,Func<string,string>> inputTransformMap = new Dictionary<string, Func<string, string>>();
        public ICriteriaBuilder<T> AddStringTransform(string propertyName, Func<string, string> transform)
        {
            inputTransformMap.Add(propertyName,transform);

            return this;
        }
       
    }
}