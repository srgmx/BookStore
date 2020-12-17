using BookStore.Domain;
using System;
using System.Threading.Tasks;

namespace BookStore.Data.Contracts
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        Task<Author> AddBookToAuthorAsync(Guid authorId, Guid bookId);
    }
}
