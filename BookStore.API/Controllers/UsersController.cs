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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();

            return Ok(users);
        }

        // GET api/users/1
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserAsync(Guid id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                return Ok(user);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserDto user)
        {
            var newUser = await _userService.AddUserAsync(user);
            var uri = Request.GetCreatedUri(newUser.Id);

            return Created(uri, newUser);
        }

        // PUT api/users/1
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, [FromBody] UserDto user)
        {
            try
            {
                user.Id = id;
                var userUpdated = await _userService.UpdateUserAsync(user);

                return Ok(userUpdated);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE api/users/1
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDto>> DeleteUser(Guid id)
        {
            try
            {
                await _userService.RemoveUserByIdAsync(id);

                return Ok();
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
