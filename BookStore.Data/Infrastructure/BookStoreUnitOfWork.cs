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
        private IBookRepopository _bookRepopository;
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
                return _userRepository ?? new UserRepository(_context);
            }
        }

        public IAuthorRepository AuthorRepository
        {
            get
            {
                return _authorRepository ?? new AuthorRepository(_context);
            }
        }

        public IBookRepopository BookRepository
        {
            get
            {
                return _bookRepopository ?? new BookRepository(_context);
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
