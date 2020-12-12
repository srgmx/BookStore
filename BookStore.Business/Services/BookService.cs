using AutoMapper;
using BookStore.Business.Contracts;
using BookStore.Business.Dto;
using BookStore.Data.Contracts;
using BookStore.Data.Specifications;
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
            var specification = new BookWithAuthorsSpecification();
            var books = await _unitOfWork.BookRepository.GetAllAsync(specification);
            var booksToReturn = _mapper.Map<IEnumerable<Book>, IEnumerable<BookDto>>(books);
            _logger.LogInformation("Books are received: {Data}", JsonSerializer.Serialize(booksToReturn));

            return booksToReturn;
        }

        public async Task<BookDto> GetBookAsync(Guid id)
        {
            var specification = new BookWithAuthorsSpecification(id);
            var book = await _unitOfWork.BookRepository.GetAsync(specification);
            CheckBookExists(book);
            var bookToReturn = _mapper.Map<Book, BookDto>(book);
            _logger.LogInformation("Book is received: {Data}", JsonSerializer.Serialize(bookToReturn));

            return bookToReturn;
        }

        public async Task<BookDto> AddBookAsync(BookToAddDto book)
        {
            var specification = new AuthorsByIdRangeSpecification(book.AuthorIds);
            var authors = await _unitOfWork.AuthorRepository.GetAllAsync(specification);
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
            var specification = new BookWithAuthorsSpecification(id);
            var book = await _unitOfWork.BookRepository.GetAsync(specification);
            CheckBookExists(book);
            _unitOfWork.BookRepository.Remove(book);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation($"Book with id {id} is removed.");

            return true;
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
