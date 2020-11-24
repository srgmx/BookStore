using AutoMapper;
using BookStore.Business.Contracts;
using BookStore.Business.Dto;
using BookStore.Data.Contracts;
using BookStore.Data.Specifications;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Business.Services
{
    public class BookService : IBookService
    {
        private readonly IBookStoreUnitOfWork _bookStoreUnitOfWork;
        private readonly IMapper _mapper;

        public BookService(
            IBookStoreUnitOfWork bookStoreUnitOfWork,
            IMapper mapper
        )
        {
            _bookStoreUnitOfWork = bookStoreUnitOfWork;
            _mapper = mapper;
        }

        public async Task<BookDto> GetBookAsync(Guid id)
        {
            var specification = new BookWithAuthorsSpecification(id);
            var book = await _bookStoreUnitOfWork.BookRepository.FindAsync(specification);
            CheckBookExists(book);
            var bookToReturn = _mapper.Map<Book, BookDto>(book);

            return bookToReturn;
        }

        public async Task<BookDto> AddBookAsync(BookToAddDto book)
        {
            var specification = new AuthorsByIdRangeSpecification(book.AuthorIds);
            var authors = await _bookStoreUnitOfWork.AuthorRepository.GetAllAsync(specification);

            if (authors.Count() != book.AuthorIds.Count())
            {
                throw new InvalidAuthorsException();
            }

            var bookToAdd = _mapper.Map<BookToAddDto, Book>(book);
            bookToAdd.Authors.AddRange(authors);
            var bookInDb = await _bookStoreUnitOfWork.BookRepository.AddAsync(bookToAdd);
            await _bookStoreUnitOfWork.SaveAsync();
            var bookToReturn = await GetBookAsync(bookInDb.Id);

            return bookToReturn;
        }

        private static void CheckBookExists(Book book)
        {
            if (book == null)
            {
                throw new RecordNotFoundException();
            }
        }
    }
}
