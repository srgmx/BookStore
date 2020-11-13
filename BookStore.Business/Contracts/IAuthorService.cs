using System.Threading.Tasks;
using BookStore.Business.Dto;

namespace BookStore.Business.Contracts
{
    public interface IAuthorService
    {
        Task<AuthorDto> AddAuthorAsync(AuthorToAddDto author);

        Task<AuthorDto> GetAuthorByIdAsync(int id);
    }
}
