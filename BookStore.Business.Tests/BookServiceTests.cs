using AutoMapper;
using BookStore.Business.Dto;
using BookStore.Business.Services;
using BookStore.Data.Contracts;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Business.Tests
{
    public class BookServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IBookRepopository> _bookRepositoryMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<ILogger<BookService>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _bookRepositoryMock = new Mock<IBookRepopository>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _unitOfWorkMock.Setup(u => u.BookRepository).Returns(_bookRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.AuthorRepository).Returns(_authorRepositoryMock.Object);
            _loggerMock = new Mock<ILogger<BookService>>();
            _mapperMock = new Mock<IMapper>();
            _bookService = new BookService(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddBookAsync_WhenAuthorNotInDb_ShouldThrowInvalidAuthorsException()
        {
            // Arrange
            var authors = GetAuthorsList();
            _authorRepositoryMock
                .Setup(b => b.GetAuthorByIdRangeAsync(It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(authors);
            var bookDto = new BookToAddDto()
            {
                Name = "Name",
                AuthorIds = new List<Guid>() { Guid.Empty }
            };

            //Act
            Func<Task> addBookAction = () => _bookService.AddBookAsync(bookDto);

            //Assert
            await Assert.ThrowsAsync<InvalidAuthorsException>(addBookAction);
        }

        private List<Author> GetAuthorsList()
        {
            return new List<Author>()
            {
                new Author() { Id = new Guid("4F94DB66-2BA4-4A7B-8360-CE012CD0AFBE") },
                new Author() { Id = new Guid("90E4F54A-ABDA-4D7A-9A61-464CCF3635A6") }
            };
        }
    }
}
