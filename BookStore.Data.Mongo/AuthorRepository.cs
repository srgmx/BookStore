using BookStore.Data.Abstraction;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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
            user.Permissions = null;

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
            var lookUpBsonDoc = GetLookUpBsonDoc();
            var matchBsonDoc = new BsonDocument("$match", new BsonDocument("_id", authorId));
            var pipeline = new BsonDocument[] { lookUpBsonDoc, matchBsonDoc };
            var aggregationCursor = await _context.Authors.AggregateAsync<BsonDocument>(pipeline);
            var authorBson = await aggregationCursor.FirstOrDefaultAsync();
            var authorJson = authorBson.ToJson();
            var author = BsonSerializer.Deserialize<Author>(authorJson);

            return author;
        }

        public async Task<IEnumerable<Author>> GetAuthorByIdRangeAsync(IEnumerable<Guid> authorsIds)
        {
            var filter = Builders<Author>.Filter.In(a => a.Id, authorsIds);
            var authorsCursor = await _context.Authors.FindAsync(filter);
            var authors = await authorsCursor.ToListAsync();

            return authors;
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            var lookUpBsonDoc = GetLookUpBsonDoc();
            var pipeline = new BsonDocument[] { lookUpBsonDoc };
            var aggregationCursor = await _context.Authors.AggregateAsync<BsonDocument>(pipeline);
            var authorsBson = await aggregationCursor.ToListAsync();
            var authorsJson = authorsBson.ToJson();
            var authors = BsonSerializer.Deserialize<List<Author>>(authorsJson);

            return authors;
        }

        public override Task RemoveAsync(Guid id)
        {
            _context.AddCommand(async () =>
            {
                // Author cascade deletion
                var authorFilter = Builders<Author>.Filter.Eq(a => a.Id, id);
                await _context.Authors.DeleteOneAsync(_context.Session, authorFilter);

                // Update books, cascade deletion of references on author
                var booksFilter = Builders<Book>.Filter
                    .ElemMatch(book => book.Authors, author => author.Id == id);
                var authorPullFilter = Builders<Author>.Filter
                    .Eq(author => author.Id, id);
                var booksUpdateDefinition = Builders<Book>.Update
                    .PullFilter(book => book.Authors, authorPullFilter);
                await _context.Books.UpdateManyAsync(_context.Session, booksFilter, booksUpdateDefinition);
            });

            return Task.CompletedTask;
        }

        private static BsonDocument GetLookUpBsonDoc()
        {
            return new BsonDocument
            {
                {
                    "$lookup",
                    new BsonDocument
                    {
                        { "from", typeof(Book).Name },
                        { "localField", "_id" },
                        { "foreignField", "authors._id" },
                        { "as", "books" }
                    }
                }
            };
        }
    }
}
