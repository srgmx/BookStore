using BookStore.Data.Abstraction;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Mongo
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(BookStoreDbContext context) : base(context)
        {
        }

        public async Task<User> AddPermissionsAsync(Guid userId, IEnumerable<string> permissions)
        {
            var userInDb = await GetByIdAsync(userId);

            if (userInDb == null)
            {
                throw new RecordNotFoundException("User was not found to add permissions.");
            }

            var newPermissions = permissions.Where(p => !userInDb.Permissions.Contains(p));
            userInDb.Permissions.AddRange(newPermissions);
            await UpdateAsync(userInDb);

            return userInDb;
        }

        public async Task<User> RemovePermissionsAsync(Guid userId, IEnumerable<string> permissions)
        {
            var userInDb = await GetByIdAsync(userId);

            if (userInDb == null)
            {
                throw new RecordNotFoundException("User was not found to remove permissions.");
            }

            var updatedPermissions = userInDb.Permissions.Where(p => !permissions.Contains(p));
            userInDb.Permissions = updatedPermissions.ToList();
            await UpdateAsync(userInDb);

            return userInDb;
        }
    }
}
