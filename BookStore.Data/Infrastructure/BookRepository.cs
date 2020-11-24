using BookStore.Data.Contracts;
using BookStore.Data.Persistance;
using BookStore.Domain;

namespace BookStore.Data.Infrastructure
{
    public class BookRepository : GenericRepository<Book>, IBookRepopository
    {
        private BookStoreDbContext _context;

        public BookRepository(BookStoreDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
