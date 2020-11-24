using BookStore.Business.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Business.Contracts
{
    public interface IBookService
    {
        public Task<IEnumerable<BookDto>> GetBooksAsync();

        public Task<BookDto> GetBookAsync(Guid id);

        public Task<BookDto> AddBookAsync(BookToAddDto book);

        public Task<bool> RemoveBookAsync(Guid id);
    }
}
