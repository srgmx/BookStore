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
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserDto user)
        {
            var newUser = await _userService.AddUserAsync(user);
            string newPath = $"{Request.Path}{newUser.Id}";

            return Created(newPath, newUser);
        }

        // PUT api/users/1
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, [FromBody] UserDto user)
        {
            user.Id = id;
            var userUpdated = await _userService.UpdateUserAsync(user);

            if (userUpdated == null)
            {
                return NotFound();
            }

            return Ok(userUpdated);
        }

        // DELETE api/users/1
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDto>> DeleteUser(Guid id)
        {
            var userRemoved = await _userService.RemoveUserByIdAsync(id);

            if (userRemoved == null)
            {
                return NotFound();
            }

            return Ok(userRemoved);
        }
    }
}
