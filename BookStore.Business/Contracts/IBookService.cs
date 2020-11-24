using BookStore.Business.Dto;
using System;
using System.Threading.Tasks;

namespace BookStore.Business.Contracts
{
    public interface IBookService
    {
        public Task<BookDto> GetBookAsync(Guid id);

        public Task<BookDto> AddBookAsync(BookToAddDto book);
    }
}
