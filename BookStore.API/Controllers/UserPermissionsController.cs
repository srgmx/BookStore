using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Business.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPermissionsController : ControllerBase
    {
        private readonly IUserPermissionsService _userPermissionsService;

        public UserPermissionsController(IUserPermissionsService userPermissionsService)
        {
            _userPermissionsService = userPermissionsService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserPermissionsAsync(Guid userId)
        {
            var userPermissions = await _userPermissionsService.GetPermissionsAsync(userId);

            return Ok(userPermissions);
        }

        [HttpPut("add/{userId}")]
        public async Task<ActionResult> AddPermissionsAsync(Guid userId, [FromBody] IEnumerable<string> permissions)
        {
            var userPermissions = await _userPermissionsService.AddPermissionsAsync(userId, permissions);

            return Ok(userPermissions);
        }

        [HttpPut("remove/{userId}")]
        public async Task<ActionResult> RemovePermissionsAsync(Guid userId, [FromBody] IEnumerable<string> permissions)
        {
            var userPermissions = await _userPermissionsService.RemovePermissionsAsync(userId, permissions);

            return Ok(userPermissions);
        }
    }
}
