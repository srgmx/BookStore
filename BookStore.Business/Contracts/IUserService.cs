using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Business.Dto;

namespace BookStore.Business.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();

        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> AddUserAsync(UserDto user);

        Task<UserDto> UpdateUserAsync(UserDto user);

        Task<UserDto> RemoveUserByIdAsync(int id);
    }
}