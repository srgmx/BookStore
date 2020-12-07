using BookStore.API.Extentions;
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
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET api/authors/
        [HttpGet]
        public async Task<ActionResult<AuthorDto>> GetAuthorByIdAsync()
        {
            var authors = await _authorService.GetAuthorsAsync();

            return Ok(authors);
        }

        // GET api/authors/1
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthorByIdAsync(Guid id)
        {
            try
            {
                var author = await _authorService.GetAuthorByIdAsync(id);

                return Ok(author);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }

        // POST api/authors
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> AddAuthorAsync(AuthorToAddDto author)
        {
            try
            {
                var newAuthor = await _authorService.AddAuthorAsync(author);
                var uri = Request.GetCreatedUri(newAuthor.Id);

                return Created(uri, newAuthor);
            }
            catch (ExistingAuthorException e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT api/authors/1
        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorDto>> UpdateAuthorAsync(Guid id, [FromBody] AuthorToUpdateDto author)
        {
            try
            {
                author.Id = id;
                var updatedAuthor = await _authorService.UpdateAuthorAsync(author);

                return Ok(updatedAuthor);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }

        // Delete api/authors/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthorAsync(Guid id)
        {
            try
            {
                await _authorService.RemoveAuthorAsync(id);

                return Ok();
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
