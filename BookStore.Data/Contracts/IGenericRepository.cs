using BookStore.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Data.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification = null);

        Task<T> GetByIdAsync(Guid id);

        Task<T> FindAsync(ISpecification<T> specification = null);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<bool> RemoveAsync(T entity);
    }
}