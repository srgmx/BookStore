using BookStore.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BookStore.Data.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            this.Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria  {get;}

        public List<Expression<Func<T, object>>> Includes { get; } =
            new List<Expression<Func<T, object>>>();

        protected virtual void AddInclude(Expression<Func<T, object>> expression)
        {
            this.Includes.Add(expression);
        }
    }
}
