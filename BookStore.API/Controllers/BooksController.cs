using BookStore.API.Extentions;
using BookStore.Business.Contracts;
using BookStore.Business.Dto;
using BookStore.Domain.Exceptions;
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
            try
            {
                var book = await _bookService.GetBookAsync(id);

                return Ok(book);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }

        // POST api/books
        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook([FromBody] BookToAddDto book)
        {
            try
            {
                var newBook = await _bookService.AddBookAsync(book);
                var uri = Request.GetCreatedUri(newBook.Id);

                return Created(uri, newBook);
            }
            catch (InvalidAuthorsException)
            {
                return BadRequest("Add author first.");
            }
        }

        // DELETE api/books/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBookAsync(Guid id)
        {
            try
            {
                var book = await _bookService.RemoveBookAsync(id);

                return Ok();
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
