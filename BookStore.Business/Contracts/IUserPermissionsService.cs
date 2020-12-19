using BookStore.Business.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Business.Contracts
{
    public interface IUserPermissionsService
    {
        Task<UserPermissionsDto> GetPermissionsAsync(Guid userId);

        Task<UserPermissionsDto> AddPermissionsAsync(Guid userId, IEnumerable<string> permissions);

        Task<UserPermissionsDto> RemovePermissionsAsync(Guid userId, IEnumerable<string> permissions);
    }
}
