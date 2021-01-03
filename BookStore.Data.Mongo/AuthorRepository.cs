using BookStore.Data.Abstraction;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Data.Mongo
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly BookStoreDbContext _context;

        public AuthorRepository(BookStoreDbContext context) : base(context)
        {
            _context = context;
        }

        public async override Task<Author> AddAsync(Author entity)
        {
            var userCursor = await _context.Users.FindAsync(u => u.Id == entity.UserId);
            var user = await userCursor.SingleOrDefaultAsync();

            if (user == null)
            {
                throw new RecordNotFoundException("Can't find user asosiated with author.");
            }

            entity.User = user;
            var author = await base.AddAsync(entity);

            return author;
        }

        public override Task<Author> UpdateAsync(Author entity)
        {
            _context.AddCommand(async () =>
            {
                var filter = Builders<Author>.Filter.Eq(a => a.Id, entity.Id);
                var updateDefinition = Builders<Author>.Update.Set(a => a.PenName, entity.PenName);
                var options = new FindOneAndUpdateOptions<Author>()
                {
                    ReturnDocument = ReturnDocument.After
                };
                entity = await _context.Authors
                    .FindOneAndUpdateAsync(_context.Session, filter, updateDefinition, options);
            });


            return Task.FromResult(entity);
        }

        public Task<Author> AddBookToAuthorAsync(Guid authorId, Guid bookId)
        {
            throw new NotImplementedException();
        }

        public async Task<Author> GetAuhorByUserIdAsync(Guid userId)
        {
            var author = await GetAsync(e => e.UserId == userId);

            return author;
        }

        public async Task<Author> GetAuthorByIdAsync(Guid authorId)
        {
            var author = await GetByIdAsync(authorId);

            return author;
        }

        public Task<IEnumerable<Author>> GetAuthorByIdRangeAsync(IEnumerable<Guid> authorsIds)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return  await GetAllAsync();
        }
    }
}
