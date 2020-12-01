using BookStore.Data.Contracts;
using BookStore.Data.Persistance;
using BookStore.Domain;

namespace BookStore.Data.Infrastructure
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly BookStoreDbContext _context;

        public AuthorRepository(BookStoreDbContext context) : base(context)
        {
            _context = context;
        }

        protected override Author SetModification(Author entityInDb, Author entity)
        {
            entityInDb.PenName = entity.PenName;
            _context.Entry(entityInDb).Property(e => e.PenName).IsModified = true;

            return entityInDb;
        }
    }
}
