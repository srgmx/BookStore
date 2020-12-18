using BookStore.Data.Contracts;
using BookStore.Data.Persistance;
using BookStore.Data.Specifications;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
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
            var specification = new AuthorWithUserAndBooksSpecification(authorId);
            var authorInDb = await GetAsync(specification);

            if (authorInDb == null)
            {
                throw new RecordNotFoundException("Can't add book to author. Author was not found.");
            }

            var bookInDb = await _context.Book.FindAsync(bookId);

            if (bookInDb == null)
            {
                throw new RecordNotFoundException("Can't add book to author. Book was not found.");
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
