using BookStore.Data.Contracts;
using BookStore.Data.Persistance;
using BookStore.Data.Specifications;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookStore.Data.Infrastructure
{
    public class BookRepository : GenericRepository<Book>, IBookRepopository
    {
        private readonly BookStoreDbContext _context;

        public BookRepository(BookStoreDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Book> AddAuthorToBookAsync(Guid bookId, Guid authorId)
        {
            var bookSpecification = new BookWithAuthorsSpecification(bookId);
            var bookInDb = await GetAsync(bookSpecification);

            if (bookInDb == null)
            {
                throw new RecordNotFoundException("Can't add author to book. Book was not found.");
            }

            var authorInDb = await _context.Author
                .Include(a => a.User)
                .SingleOrDefaultAsync(a => a.Id == authorId);

            if (authorInDb == null)
            {
                throw new RecordNotFoundException("Can't add author to book. Author was not found.");
            }

            bookInDb.Authors.Add(authorInDb);

            return bookInDb;
        }
    }
}
