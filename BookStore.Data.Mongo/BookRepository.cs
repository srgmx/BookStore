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
                // Add book. Store author and user unique identifiers only
                var authorsWithKeyFields = entity.Authors.Select(author => new Author()
                {
                    Id = author.Id,
                    UserId = author.UserId,
                    Books = null
                });
                entity.Authors = authorsWithKeyFields.ToList();
                await _context.Books.InsertOneAsync(_context.Session, entity);
                
                // Update author and bind created book
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
            // Primitive implementation
            //var book = await base.GetByIdAsync(bookId);
            //var authorFilter = Builders<Author>.Filter.In(author => author.Id, 
            //    book.Authors.Select(bookAuthor => bookAuthor.Id));
            //var authorsCursor = await _context.Authors.FindAsync(authorFilter);
            //book.Authors = await authorsCursor.ToListAsync();
            //return book;

            // Query on MongoDB implementation
            var lookUp = new BsonDocument
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
            var match = new BsonDocument("$match", new BsonDocument("_id", bookId));
            var pipeline = new[] { lookUp, match };
            var aggregationCursor = await _context.Books.AggregateAsync<BsonDocument>(pipeline);
            var bookBson = await aggregationCursor.FirstOrDefaultAsync();
            var bookJson = bookBson.ToJson();
            var book = BsonSerializer.Deserialize<Book>(bookJson);

            return book;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            // LINQ Exercise
            //var result = _context.Books.AsQueryable()
            //      .SelectMany(book => book.Authors, (book, author) => new
            //      {
            //          BookId = book.Id,
            //          BookName = book.Name,
            //          AuthorId = author.Id
            //      })
            //      .Join(
            //            _context.Authors.AsQueryable(),
            //            left => left.AuthorId,
            //            right => right.Id,
            //            (left, right) => new
            //            {
            //                BookId = left.BookId,
            //                BookName = left.BookName,
            //                AuthorId = right.Id,
            //                AuthorUserId = right.UserId,
            //                AuthorPenName = right.PenName,
            //                AuthorUser = right.User,
            //                AuthorBooks = right.Books
            //            }
            //      )
            //      .ToList();

            // Primitive implementation
            //var books = await GetAllAsync();
            //foreach (var book in books)
            //{
            //    var authorIds = book.Authors.Select(a => a.Id);
            //    var filter = Builders<Author>.Filter.In(a => a.Id, authorIds);
            //    var authorsCursor = await _context.Authors.FindAsync(filter);
            //    var authors = await authorsCursor.ToListAsync();
            //    book.Authors = authors;
            //}
            //return books;

            // Query on MongoDB implementation
            var lookUp = new BsonDocument
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
            var pipeline = new[] { lookUp };
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
                var bookPullFilter = Builders<Book>.Filter.Eq(b => b.Id, id);
                var auhtorFilter = Builders<Author>.Filter
                    .ElemMatch(author => author.Books, book => book.Id == id);
                var updateDefiniton = Builders<Author>.Update
                    .PullFilter(author => author.Books, bookPullFilter);
                await _context.Authors.UpdateManyAsync(_context.Session, auhtorFilter, updateDefiniton);
                await _context.Books.DeleteOneAsync(_context.Session, bookPullFilter);
            });

            return Task.CompletedTask;
        }
    }
}
