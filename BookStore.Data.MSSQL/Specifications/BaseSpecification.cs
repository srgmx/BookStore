using BookStore.Data.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BookStore.Data.MSSQL.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } =
            new List<Expression<Func<T, object>>>();

        public List<string> IncludeStrings { get; } =
            new List<string>();

        protected virtual void AddInclude(Expression<Func<T, object>> expression)
        {
            Includes.Add(expression);
        }

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
    }
}
