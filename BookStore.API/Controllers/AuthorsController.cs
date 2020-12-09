using BookStore.API.Extentions;
using BookStore.Business.Contracts;
using BookStore.Business.Dto;
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
            var author = await _authorService.GetAuthorByIdAsync(id);

            return Ok(author);
        }

        // POST api/authors
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> AddAuthorAsync(AuthorToAddDto author)
        {
            var newAuthor = await _authorService.AddAuthorAsync(author);
            var uri = Request.GetCreatedUri(newAuthor.Id);

            return Created(uri, newAuthor);
        }

        // PUT api/authors/1
        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorDto>> UpdateAuthorAsync(Guid id, [FromBody] AuthorToUpdateDto author)
        {
            author.Id = id;
            var updatedAuthor = await _authorService.UpdateAuthorAsync(author);

            return Ok(updatedAuthor);
        }

        // Delete api/authors/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthorAsync(Guid id)
        {
            await _authorService.RemoveAuthorAsync(id);

            return Ok();
        }
    }
}
