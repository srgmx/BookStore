using BookStore.Business.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

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