using AutoMapper;
using BookStore.Business.Contracts;
using BookStore.Business.Dto;
using BookStore.Data.Contracts;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookStore.Business.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<BookService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BookDto>> GetBooksAsync()
        {
            var books = await _unitOfWork.BookRepository.GetBooksAsync();
            var booksToReturn = _mapper.Map<IEnumerable<Book>, IEnumerable<BookDto>>(books);
            _logger.LogInformation("Books are received: {Data}", JsonSerializer.Serialize(booksToReturn));

            return booksToReturn;
        }

        public async Task<BookDto> GetBookAsync(Guid id)
        {
            var book = await _unitOfWork.BookRepository.GetBookByIdAsync(id);
            CheckBookExists(book);
            var bookToReturn = _mapper.Map<Book, BookDto>(book);
            _logger.LogInformation("Book is received: {Data}", JsonSerializer.Serialize(bookToReturn));

            return bookToReturn;
        }

        public async Task<BookDto> AddBookAsync(BookToAddDto book)
        {
            var authors = await _unitOfWork.AuthorRepository.GetAuthorByIdRangeAsync(book.AuthorIds);
            _logger.LogInformation("Authors are available in the database: {Data}", authors);

            if (authors.Count() != book.AuthorIds.Count())
            {
                var message = "Invalid author of the book in the list. All authors must exist in the system.";
                _logger.LogWarning("Invalid author of the book in the list: {Data}", JsonSerializer.Serialize(book));

                throw new InvalidAuthorsException(message);
            }

            var bookToAdd = _mapper.Map<BookToAddDto, Book>(book);
            bookToAdd.Authors.AddRange(authors);
            var bookInDb = await _unitOfWork.BookRepository.AddAsync(bookToAdd);
            await _unitOfWork.SaveAsync();
            var bookToReturn = await GetBookAsync(bookInDb.Id);
            _logger.LogInformation("Book is added: {Data}", JsonSerializer.Serialize(bookToReturn));

            return bookToReturn;
        }

        public async Task<bool> RemoveBookAsync(Guid id)
        {
            await _unitOfWork.BookRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation($"Book with id {id} is removed.");

            return true;
        }

        public async Task<BookDto> AddAuthorToBookAsync(Guid bookId, Guid authorId)
        {
            try
            {
                var bookInDb = await _unitOfWork.BookRepository.AddAuthorToBookAsync(bookId, authorId);
                await _unitOfWork.SaveAsync();
                var bookToReturn = _mapper.Map<Book, BookDto>(bookInDb);
                _logger.LogInformation($"Author with id {authorId} was added for book with id {bookId}.");

                return bookToReturn;
            }
            catch (RecordNotFoundException e)
            {
                _logger.LogWarning(e.Message);

                throw;
            }
        }

        private void CheckBookExists(Book book)
        {
            if (book == null)
            {
                var message = "Book was not found.";
                _logger.LogWarning(message);

                throw new RecordNotFoundException(message);
            }
        }
    }
}
