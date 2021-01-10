using BookStore.Data.Abstraction;
using BookStore.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Mongo
{
    public class BookRepository : GenericRepository<Book>, IBookRepopository
    {
        private readonly BookStoreDbContext _context;

        public BookRepository(BookStoreDbContext context) : base(context)
        {
            _context = context;
        }

        public override Task<Book> AddAsync(Book entity)
        {
            _context.AddCommand(async () =>
            {
                // Add book. Store author unique identifier. Stor user unique identifier
                var authorsWithKeyFields = entity.Authors.Select(author => new Author()
                {
                    Id = author.Id,
                    UserId = author.UserId,
                    Books = null
                });
                entity.Authors = authorsWithKeyFields.ToList();
                await _context.Books.InsertOneAsync(_context.Session, entity);
                
                // Update author. Store book unique identifier
                var bookWithKeyField = new Book()
                {
                    Id = entity.Id,
                    Authors = null
                };
                var authorIds = entity.Authors.Select(bookAuthor => bookAuthor.Id);
                var authorsFilter = Builders<Author>.Filter.In(author => author.Id, authorIds);
                var authorsUpdateDefinition = Builders<Author>.Update.Push(author => author.Books, bookWithKeyField);
                await _context.Authors.UpdateManyAsync(_context.Session, authorsFilter, authorsUpdateDefinition);
            });

            return Task.FromResult(entity);
        }

        public Task<Book> AddAuthorToBookAsync(Guid bookId, Guid authorId)
        {
            throw new NotImplementedException();
        }

        public async Task<Book> GetBookByIdAsync(Guid bookId)
        {
            var lookUpBsonDoc = GetLookUpBsonDoc();
            var matchBsonDoc = new BsonDocument("$match", new BsonDocument("_id", bookId));
            var pipeline = new[] { lookUpBsonDoc, matchBsonDoc };
            var aggregationCursor = await _context.Books.AggregateAsync<BsonDocument>(pipeline);
            var bookBson = await aggregationCursor.FirstOrDefaultAsync();
            var bookJson = bookBson.ToJson();
            var book = BsonSerializer.Deserialize<Book>(bookJson);

            return book;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            var lookUpBsonDoc = GetLookUpBsonDoc();
            var pipeline = new[] { lookUpBsonDoc };
            var aggregationCursor = await _context.Books.AggregateAsync<BsonDocument>(pipeline);
            var booksBson = await aggregationCursor.ToListAsync();
            var booksJson = booksBson.ToJson();
            var books = BsonSerializer.Deserialize<List<Book>>(booksJson);

            return books;
        }

        public override Task RemoveAsync(Guid id)
        {
            _context.AddCommand(async () =>
            {
                // Delete book
                var bookFilter = Builders<Book>.Filter.Eq(b => b.Id, id);
                await _context.Books.DeleteOneAsync(_context.Session, bookFilter);

                // Update auhtors, cascade deletion of references on book
                var auhtorsFilter = Builders<Author>.Filter
                    .ElemMatch(author => author.Books, book => book.Id == id);
                var authorsUpdateDefiniton = Builders<Author>.Update
                    .PullFilter(author => author.Books, bookFilter);
                await _context.Authors.UpdateManyAsync(_context.Session, auhtorsFilter, authorsUpdateDefiniton);

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
                        { "from", typeof(Author).Name },
                        { "localField", "authors._id" },
                        { "foreignField", "_id" },
                        { "as", "authors" }
                    }
                }
            };
        }
    }
}
