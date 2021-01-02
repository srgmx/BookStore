using BookStore.Data.Abstraction;
using BookStore.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Data.Mongo
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(BookStoreDbContext context) : base(context)
        {
        }

        public Task<User> AddPermissionsAsync(Guid userId, IEnumerable<string> permissions)
        {
            throw new NotImplementedException();
        }

        public Task<User> RemovePermissionsAsync(Guid userId, IEnumerable<string> permissions)
        {
            throw new NotImplementedException();
        }
    }
}
