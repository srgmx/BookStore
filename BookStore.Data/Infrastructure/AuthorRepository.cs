using BookStore.Data.Contracts;
using BookStore.Data.Persistance;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookStore.Data.Infrastructure
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly BookStoreDbContext _context;

        public AuthorRepository(BookStoreDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Author> AddBookToAuthorAsync(Guid authorId, Guid bookId)
        {
            var authorInDb = await _context.Author
                    .Include(a => a.User)
                    .Include(a => a.Books)
                    .SingleOrDefaultAsync(a => a.Id == authorId);
            var bookInDb = await _context.Book.FindAsync(bookId);

            if (authorInDb == null)
            {
                var message = "Can't add book to author. Author was not found.";

                throw new RecordNotFoundException(message);
            }

            if (bookInDb == null)
            {
                var message = "Can't add book to author. Book was not found.";

                throw new RecordNotFoundException(message);
            }

            authorInDb.Books.Add(bookInDb);

            return authorInDb;
        }

        protected override Author SetModification(Author entityInDb, Author entity)
        {
            entityInDb.PenName = entity.PenName;
            _context.Entry(entityInDb).Property(e => e.PenName).IsModified = true;

            return entityInDb;
        }
    }
}
