using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.DTOs.Users;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Models.Entities;
using AssetManagementSystem.Repositories;
using AssetManagementSystem.Services;

namespace AssetManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        private readonly UserService _userService;
        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<User>>> Get([FromQuery] GetUsersRequest request)
        {
            var result = await _userService.Get(request);
            return result.Succeeded ? Ok(result.Value) : ToActionResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] CreateUserRequest request)
        {
            var result = await _userService.Create(request);
            return result.Succeeded ? 
                CreatedAtAction(nameof(Get), new { id = result.Value }, null) : 
                ToActionResult(result);
        }

        [HttpGet("fields")]
        public ActionResult<UserFields> GetFields()
        {
            return Ok(new UserFields());
        }

        [HttpPatch("{id:guid}/role")]
        public async Task<ActionResult<User>> UpdateRole(Guid id, [FromBody] UpdateUserRoleRequest request)
        {

            var result = await _userService.UpdateRole(id, request);
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/active")]
        public async Task<IActionResult> UpdateActive(Guid id, UpdateUserActiveRequest request)
        {
            var result = await _userService.UpdateActive(id, request);
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }
    }
}