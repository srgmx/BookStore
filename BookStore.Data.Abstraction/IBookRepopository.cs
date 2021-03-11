using BookStore.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Data.Abstraction
{
    public interface IBookRepopository : IGenericRepository<Book>
    {
        Task<Book> AddAuthorToBookAsync(Guid bookId, Guid authorId);

        Task<IEnumerable<Book>> GetBooksAsync();

        Task<IEnumerable<Book>> GetBooksByIdRangeAsync(IEnumerable<Guid> bookIds);

        Task<Book> GetBookByIdAsync(Guid bookId);
    }
}
