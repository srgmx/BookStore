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

            var newPermissions = permissions.Where(permission => !userInDb.Permissions.Contains(permission));
            userInDb.Permissions.AddRange(newPermissions);
            userInDb = await base.UpdateAsync(userInDb);

            return userInDb;
        }

        public override async Task<User> UpdateAsync(User entity)
        {
            _context.AddCommand(async () =>
            {
                // Update user
                var userFilter = Builders<User>.Filter.Eq(user => user.Id, entity.Id);
                var userUpdateDefinition = Builders<User>.Update.Combine(
                    Builders<User>.Update.Set(user => user.FirstName, entity.FirstName),
                    Builders<User>.Update.Set(user => user.LastName, entity.LastName)
                );
                var userOptions = new FindOneAndUpdateOptions<User>()
                {
                    ReturnDocument = ReturnDocument.After
                };
                entity = await _context.Users
                    .FindOneAndUpdateAsync(_context.Session, userFilter, userUpdateDefinition, userOptions);
            });
            _context.AddCommand(async () =>
            {
                // Update author if exists
                var authorFilter = Builders<Author>.Filter.Eq(author => author.UserId, entity.Id);
                var authorUpdateDefinition = Builders<Author>.Update.Combine(
                    Builders<Author>.Update.Set(author => author.User.FirstName, entity.FirstName),
                    Builders<Author>.Update.Set(author => author.User.LastName, entity.LastName)
                );
                await _context.Authors.UpdateOneAsync(_context.Session, authorFilter, authorUpdateDefinition);
            });
            await _context.SaveChangesAsync();

            return entity;
        }

        public override Task RemoveAsync(Guid id)
        {
            _context.AddCommand(async () =>
            {
                // Delete user
                var userFilter = Builders<User>.Filter.Eq(user => user.Id, id);
                await _context.Users.DeleteOneAsync(_context.Session, userFilter);
            });
            _context.AddCommand(async () =>
            {
                // Author cascade deletion
                var authorFilter = Builders<Author>.Filter.Eq(author => author.UserId, id);
                await _context.Authors.DeleteOneAsync(_context.Session, authorFilter);
            });
            _context.AddCommand(async () =>
            {
                // Update books, cascade deletion of references on author
                var booksFilter = Builders<Book>.Filter.ElemMatch(book => book.Authors, author => author.UserId == id);
                var authorPullFilter = Builders<Author>.Filter.Eq(author => author.UserId, id);
                var booksUpdateDefinition = Builders<Book>.Update.PullFilter(book => book.Authors, authorPullFilter);
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

            var updatedPermissions = userInDb.Permissions.Where(userPermission => !permissions.Contains(userPermission));
            userInDb.Permissions = updatedPermissions.ToList();
            userInDb = await base.UpdateAsync(userInDb);

            return userInDb;
        }
    }
}
