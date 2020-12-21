using BookStore.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Data.Contracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> AddPermissionsAsync(Guid userId, IEnumerable<string> permissions);

        Task<User> RemovePermissionsAsync(Guid userId, IEnumerable<string> permissions);
    }
}
