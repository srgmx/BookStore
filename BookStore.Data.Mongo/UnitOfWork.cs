using BookStore.Data.Abstraction;
using System.Threading.Tasks;

namespace BookStore.Data.Mongo
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookStoreDbContext _context;
        private IUserRepository _userRepository;
        private IAuthorRepository _authorRepository;
        private IBookRepopository _bookRepopository;

        public UnitOfWork(BookStoreDbContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }

                return _userRepository;
            }
        }

        public IAuthorRepository AuthorRepository
        {
            get
            {
                if (_authorRepository == null)
                {
                    _authorRepository = new AuthorRepository(_context);
                }

                return _authorRepository;
            }
        }

        public IBookRepopository BookRepository => throw new System.NotImplementedException();

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
