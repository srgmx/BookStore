using BookStore.API.Extentions;
using BookStore.Business.Contracts;
using BookStore.Business.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET api/books/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksAsync()
        {
            var books = await _bookService.GetBooksAsync();

            return Ok(books);
        }

        // GET api/books/1
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBookAsync(Guid id)
        {
            var book = await _bookService.GetBookAsync(id);

            return Ok(book);
        }

        [HttpPost("idRange")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksByIdRangeAsync([FromBody] IEnumerable<Guid> bookIds)
        {
            var books = await _bookService.GetBooksByIdRangeAsync(bookIds);

            return Ok(books);
        }

        // POST api/books
        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBookAsync([FromBody] BookToAddDto book)
        {
            var newBook = await _bookService.AddBookAsync(book);
            var uri = Request.GetCreatedUri(newBook.Id);

            return Created(uri, newBook);
        }

        // DELETE api/books/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBookAsync(Guid id)
        {
            await _bookService.RemoveBookAsync(id);

            return Ok();
        }

        // PUT api/books/1/author/1
        [HttpPut("{bookId}/authors/{authorId}")]
        public async Task<ActionResult> AddAuthorToBook(Guid bookId, Guid authorId)
        {
            var book = await _bookService.AddAuthorToBookAsync(bookId, authorId);

            return Ok(book);
        }
    }
}
