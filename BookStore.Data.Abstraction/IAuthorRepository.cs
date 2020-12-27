using BookStore.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Data.Contracts
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        Task<Author> AddBookToAuthorAsync(Guid authorId, Guid bookId);

        Task<IEnumerable<Author>> GetAuthorsAsync();

        Task<Author> GetAuthorByIdAsync(Guid authorId);

        Task<Author> GetAuhorByUserIdAsync(Guid userId);

        Task<IEnumerable<Author>> GetAuthorByIdRangeAsync(IEnumerable<Guid> authorsIds);
    }
} 
