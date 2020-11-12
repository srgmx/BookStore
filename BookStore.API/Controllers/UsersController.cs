using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Business.Contracts;
using BookStore.Business.Dto;

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
            return this.Ok(users);
        }

        // GET api/users/1
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserAsync(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null) return this.NotFound();

            return this.Ok(user);
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserDto user)
        {
            var newUser = await _userService.AddUserAsync(user);
            return this.Ok(newUser);
        }

        // PUT api/users/1
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UserDto user)
        {
            user.Id = id;

            var userUpdated = await _userService.UpdateUserAsync(user);

            if (userUpdated == null) return this.NotFound();

            return this.Ok(userUpdated);
        }

        // DELETE api/users/1
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDto>> DeleteUser(int id)
        {
            var userRemoved = await _userService.RemoveUserByIdAsync(id);

            if (userRemoved == null) return this.NotFound();

            return this.Ok(userRemoved);
        }
    }
}
