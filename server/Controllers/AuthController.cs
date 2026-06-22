using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AssetManagementSystem.DTOs.Auth;
using AssetManagementSystem.Extensions;
using AssetManagementSystem.Models.Entities;
using AssetManagementSystem.Repositories;
using AssetManagementSystem.Services;

namespace AssetManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly TokenService _tokenService;

        public AuthController(UserRepository userRepository, TokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest loginData)
        {
            // There is intentionally no check for password.
            // Managing passwords is outside the scope of this project.
            User? user = await _userRepository.GetUserByEmailAsync(loginData.EmailAddress);

            if (user == null || !user.IsActive) return Unauthorized();

            string token = _tokenService.CreateToken(user);

            await _userRepository.UpdateLastLoginAsync(user.Id);

            return Ok(new { AuthorizationToken = token });
        }

        [Authorize]
        [HttpGet("Me")]
        public ActionResult<User> Me()
        {
            User user = HttpContext.GetCurrentUser();

            return Ok(user);
        }

        [Authorize(Policy = "AssetManager+")]
        [HttpGet("AssetManager")]
        public ActionResult<User> Manager()
        {
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Admin")]
        public ActionResult<User> Admin()
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("Debug")]
        public IActionResult Debug()
        {
            return Ok(User.Claims.Select(c => new
            {
                c.Type,
                c.Value
            }));
        }
    }
}
