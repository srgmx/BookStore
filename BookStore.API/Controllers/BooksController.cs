using BookStore.Business.Contracts;
using BookStore.Business.Dto;
using BookStore.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
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

        // GET api/books/1
        [HttpGet("{id}")]
        public async Task<ActionResult> GetBookAsync(Guid id)
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
        public async Task<ActionResult> CreateBook([FromBody] BookToAddDto book)
        {
            try
            {
                var newBook = await _bookService.AddBookAsync(book);
                var resourcePath = $"{Request.Path}/{newBook.Id}";

                return Created(resourcePath, newBook);
            }
            catch (InvalidAuthorsException)
            {
                return BadRequest("Add author first.");
            }
        }
    }
}
