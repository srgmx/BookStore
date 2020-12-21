using BookStore.Domain;
using System;
using System.Threading.Tasks;

namespace BookStore.Data.Contracts
{
    public interface IBookRepopository : IGenericRepository<Book>
    {
        Task<Book> AddAuthorToBookAsync(Guid bookId, Guid authorId);
    }
}
