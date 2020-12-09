using AutoMapper;
using BookStore.Business.Contracts;
using BookStore.Business.Dto;
using BookStore.Data.Contracts;
using BookStore.Data.Specifications;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Business.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookService(
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> GetBooksAsync()
        {
            var specification = new BookWithAuthorsSpecification();
            var books = await _unitOfWork.BookRepository.GetAllAsync(specification);

            return _mapper.Map<IEnumerable<Book>, IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetBookAsync(Guid id)
        {
            var specification = new BookWithAuthorsSpecification(id);
            var book = await _unitOfWork.BookRepository.GetAsync(specification);
            CheckBookExists(book);
            var bookToReturn = _mapper.Map<Book, BookDto>(book);

            return bookToReturn;
        }

        public async Task<BookDto> AddBookAsync(BookToAddDto book)
        {
            var specification = new AuthorsByIdRangeSpecification(book.AuthorIds);
            var authors = await _unitOfWork.AuthorRepository.GetAllAsync(specification);

            if (authors.Count() != book.AuthorIds.Count())
            {
                var message = "Invalid author in the list. All authors must exist in the system.";

                throw new InvalidAuthorsException(message);
            }

            var bookToAdd = _mapper.Map<BookToAddDto, Book>(book);
            bookToAdd.Authors.AddRange(authors);
            var bookInDb = await _unitOfWork.BookRepository.AddAsync(bookToAdd);
            await _unitOfWork.SaveAsync();
            var bookToReturn = await GetBookAsync(bookInDb.Id);

            return bookToReturn;
        }

        public async Task<bool> RemoveBookAsync(Guid id)
        {
            var specification = new BookWithAuthorsSpecification(id);
            var book = await _unitOfWork.BookRepository.GetAsync(specification);
            CheckBookExists(book);
            _unitOfWork.BookRepository.Remove(book);
            await _unitOfWork.SaveAsync();

            return true;
        }

        private static void CheckBookExists(Book book)
        {
            if (book == null)
            {
                var message = "Book was not found.";

                throw new RecordNotFoundException(message);
            }
        }
    }
}
