using BookStore.Data.Contracts;
using BookStore.Data.Persistance;
using BookStore.Domain;

namespace BookStore.Data.Infrastructure
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private BookStoreDbContext _context;
        public AuthorRepository(BookStoreDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
