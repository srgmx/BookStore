using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Business.Contracts;
using BookStore.Business.Dto;
using BookStore.Business.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        // POST api/authors
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> AddAuthorAsync(AuthorToAddDto author)
        {
            try
            {
                var newAuthor = await _authorService.AddAuthorAsync(author);
                var newPath = $"{Request.Path}{newAuthor.Id}";

                return this.Created(newPath, newAuthor);
            }
            catch(ExistingAuthorException ex)
            {
                return this.BadRequest(ex.Message);
            }

        }

        // GET api/authors/1
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthorByIdAsync(int id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);

            if (author == null) return this.NotFound();

            return this.Ok(author);
        }
    }
}
