using BookStore.Data.Contracts;
using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookStore.Data.MSSQL.Specifications
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(
            IQueryable<T> inputQuery, 
            ISpecification<T> specification
        ) 
        {
            var query = inputQuery;

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            query = specification.Includes.Aggregate(
                query,
                (current, include) => current.Include(include)
            );

            query = specification.IncludeStrings.Aggregate(
                query,
                (current, include) => current.Include(include)
            );

            return query;
        }
    }
}
