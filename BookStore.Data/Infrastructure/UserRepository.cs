using BookStore.Data.Contracts;
using BookStore.Data.Persistance;
using BookStore.Domain;

namespace BookStore.Data.Infrastructure
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private BookStoreDbContext _context;
        public UserRepository(BookStoreDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
