using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Data.Contracts
{
    public interface IGenericRepository<T> where T: class
    {
        Task<T> FindByIdAsync(int id);
        Task<T> FindAsync(ISpecification<T> specification);
        Task<IEnumerable<T>> FindAllAsync(ISpecification<T> specification);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> RemoveAsync(T entity);
    }
}