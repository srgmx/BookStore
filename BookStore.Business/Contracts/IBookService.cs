using BookStore.Business.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Business.Contracts
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetBooksAsync();

        Task<BookDto> GetBookAsync(Guid id);

        Task<IEnumerable<BookDto>> GetBooksByIdRangeAsync(IEnumerable<Guid> bookIds);

        Task<BookDto> AddBookAsync(BookToAddDto book);

        Task<bool> RemoveBookAsync(Guid id);

        Task<BookDto> AddAuthorToBookAsync(Guid bookId, Guid authorId);
    }
}
