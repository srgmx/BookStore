using BookStore.Business.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Business.Contracts
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAuthorsAsync();

        Task<AuthorDto> GetAuthorByIdAsync(Guid id);

        Task<AuthorDto> AddAuthorAsync(AuthorToAddDto author);

        Task<AuthorDto> UpdateAuthorAsync(AuthorToUpdateDto author);

        Task<bool> RemoveAuthorAsync(Guid id);

        Task<AuthorDto> AddBookToAuthor(Guid authorId, Guid bookId);
    }
}
