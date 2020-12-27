using BookStore.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookStore.Data.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> criteria = null);

        Task<T> GetByIdAsync(Guid id);

        Task<T> GetAsync(Expression<Func<T, bool>> criteria);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task RemoveAsync(T entity);

        Task RemoveAsync(Guid id);
    }
}
