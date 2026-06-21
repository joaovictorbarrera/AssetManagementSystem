using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.DTOs.Users;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Models.Entities;
using AssetManagementSystem.Repositories;

namespace AssetManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<User>>> GetUsers([FromQuery] GetUsersRequest request)
        {
            PagedResponse<User> users = await _userRepository.GetUsersAsync(request);

            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserRequest request)
        {
            if (!Enum.IsDefined(typeof(Role), request.Role))
                return BadRequest("Invalid role");

            bool userExists = await _userRepository.GetUserByEmailAsync(request.EmailAddress) != null;
            if (userExists) return BadRequest("Email Address is taken");

            User user = await _userRepository.CreateUserAsync(request);

            return Ok(user);
        }

        [HttpPatch("{id:guid}/role")]
        public async Task<ActionResult<User>> UpdateUserRole(Guid id, [FromBody] UpdateUserRoleRequest request)
        {
            if (!Enum.IsDefined(typeof(Role), request.Role))
                return BadRequest("Invalid role");

            bool success = await _userRepository.UpdateUserRole(id, request.Role);

            if (!success) return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}/active")]
        public async Task<IActionResult> UpdateUserActiveStatus(Guid id, UpdateUserActiveRequest request)
        {
            bool success = await _userRepository.UpdateUserActive(id, request.IsActive);

            if (!success) return NotFound();

            return NoContent();
        }
    }
}