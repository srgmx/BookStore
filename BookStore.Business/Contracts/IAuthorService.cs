using BookStore.Business.Dto;
using System;
using System.Threading.Tasks;

namespace BookStore.Business.Contracts
{
    public interface IAuthorService
    {
        Task<AuthorDto> AddAuthorAsync(AuthorToAddDto author);

        Task<AuthorDto> GetAuthorByIdAsync(Guid id);
    }
}
