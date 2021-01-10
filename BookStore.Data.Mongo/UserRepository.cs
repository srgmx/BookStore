using BookStore.Data.Abstraction;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Mongo
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly BookStoreDbContext _context;

        public UserRepository(BookStoreDbContext context) : base(context)
        {
            _context = context;
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

        public override Task RemoveAsync(Guid id)
        {
            _context.AddCommand(async () =>
            {
                // Delete user
                var userFilter = Builders<User>.Filter.Eq(u => u.Id, id);
                await _context.Users.DeleteOneAsync(_context.Session, userFilter);

                // Author cascade deletion
                var authorFilter = Builders<Author>.Filter.Eq(a => a.UserId, id);
                await _context.Authors.DeleteOneAsync(_context.Session, authorFilter);

                // Update books, cascade deletion of references on author
                var booksFilter = Builders<Book>.Filter
                    .ElemMatch(book => book.Authors, author => author.UserId == id);
                var authorPullFilter = Builders<Author>.Filter
                    .Eq(author => author.UserId, id);
                var booksUpdateDefinition = Builders<Book>.Update
                    .PullFilter(book => book.Authors, authorPullFilter);
                await _context.Books.UpdateManyAsync(_context.Session, booksFilter, booksUpdateDefinition);
            });

            return Task.CompletedTask;
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
