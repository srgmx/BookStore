using BookStore.Data.Contracts;
using BookStore.Data.Persistance;
using System;
using System.Threading.Tasks;

namespace BookStore.Data.Infrastructure
{
    public class BookStoreUnitOfWork : IBookStoreUnitOfWork, IDisposable
    {
        private IUserRepository _userRepository;
        private IAuthorRepository _authorRepository;
        private readonly BookStoreDbContext _context;
        private bool _isDisposed;

        public BookStoreUnitOfWork(BookStoreDbContext context)
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

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
