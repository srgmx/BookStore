using BookStore.Data.Contracts;
using BookStore.Data.Persistance;
using BookStore.Data.Specifications;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Author>> GetAuthorByIdRangeAsync(IEnumerable<Guid> authorsIds)
        {
            var specification = new AuthorsByIdRangeSpecification(authorsIds);
            var authors = await GetAllAsync(specification);

            return authors;
        }

        public async Task<Author> GetAuhorByUserIdAsync(Guid userId)
        {
            var specification = new AuthorByUserIdSpecification(userId);
            var author = await GetAsync(specification);

            return author;
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            var specification = new AuthorWithUserAndBooksSpecification();
            var authors = await GetAllAsync(specification);

            return authors;
        }

        public async Task<Author> GetAuthorByIdAsync(Guid authorId)
        {
            var specification = new AuthorWithUserAndBooksSpecification(authorId);
            var author = await GetAsync(specification);

            return author;
        }

        protected override Author SetModification(Author entityInDb, Author entity)
        {
            entityInDb.PenName = entity.PenName;
            _context.Entry(entityInDb).Property(e => e.PenName).IsModified = true;

            return entityInDb;
        }
    }
}
