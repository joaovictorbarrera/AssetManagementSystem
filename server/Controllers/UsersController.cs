using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreatlockerAssetManagementSystem.DTOs.Pagination;
using ThreatlockerAssetManagementSystem.DTOs.Users;
using ThreatlockerAssetManagementSystem.Models.Entities;
using ThreatlockerAssetManagementSystem.Repositories;

namespace ThreatlockerAssetManagementSystem.Controllers
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
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpPatch("{id}/role")]
        public async Task<IActionResult> UpdateUserRole(Guid id)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpPatch("{id}/active")]
        public async Task<IActionResult> UpdateUserActiveStatus(Guid id)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}