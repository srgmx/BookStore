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
            var userInDbCursor = await _context.Users.FindAsync(user => user.Id == entity.UserId);
            var userInDb = await userInDbCursor.SingleOrDefaultAsync();

            if (userInDb == null)
            {
                throw new RecordNotFoundException("Can't find user asosiated with author.");
            }

            userInDb.Permissions = null;
            entity.User = userInDb;
            var authorInDb = await base.AddAsync(entity);

            return authorInDb;
        }

        public async override Task<Author> UpdateAsync(Author entity)
        {
            _context.AddCommand(async () =>
            {
                var filter = Builders<Author>.Filter.Eq(auhor => auhor.Id, entity.Id);
                var updateDefinition = Builders<Author>.Update.Set(auhor => auhor.PenName, entity.PenName);
                var options = new FindOneAndUpdateOptions<Author>()
                {
                    ReturnDocument = ReturnDocument.After
                };
                entity = await _context.Authors
                    .FindOneAndUpdateAsync(_context.Session, filter, updateDefinition, options);
            });
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Author> AddBookToAuthorAsync(Guid authorId, Guid bookId)
        {
            var authorInDb = await GetByIdAsync(authorId);

            if (authorInDb == null)
            {
                throw new RecordNotFoundException("Can't add book to author. Author was not found.");
            }

            var bookInDbCursor = await _context.Books.FindAsync(book => book.Id == bookId);
            var bookInDb = await bookInDbCursor.SingleOrDefaultAsync();

            if (bookInDb == null)
            {
                throw new RecordNotFoundException("Can't add book to author. Book was not found.");
            }

            var bookWithKeyField = new Book()
            {
                Id = bookId,
                Authors = null
            };
            authorInDb.Books.Add(bookWithKeyField);
            authorInDb = await base.UpdateAsync(authorInDb);

            return authorInDb;
        }

        public async Task<Author> GetAuhorByUserIdAsync(Guid userId)
        {
            var authorInDb = await GetAsync(author => author.UserId == userId);

            return authorInDb;
        }

        public async Task<Author> GetAuthorByIdAsync(Guid authorId)
        {
            var lookUpBsonDoc = GetLookUpBsonDoc();
            var matchBsonDoc = new BsonDocument("$match", new BsonDocument("_id", authorId));
            var pipeline = new BsonDocument[] { lookUpBsonDoc, matchBsonDoc };
            var aggregationCursor = await _context.Authors.AggregateAsync<BsonDocument>(pipeline);
            var authorBson = await aggregationCursor.FirstOrDefaultAsync();

            if (authorBson == null)
            {
                return null;
            }

            var authorJson = authorBson.ToJson();
            var author = BsonSerializer.Deserialize<Author>(authorJson);

            return author;
        }

        public async Task<IEnumerable<Author>> GetAuthorByIdRangeAsync(IEnumerable<Guid> authorsIds)
        {
            var authorsFilter = Builders<Author>.Filter.In(author => author.Id, authorsIds);
            var authorsCursor = await _context.Authors.FindAsync(authorsFilter);
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
                var authorFilter = Builders<Author>.Filter.Eq(author => author.Id, id);
                await _context.Authors.DeleteOneAsync(_context.Session, authorFilter);
            });
            _context.AddCommand(async () =>
            {
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
