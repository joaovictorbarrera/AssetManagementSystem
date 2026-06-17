using Microsoft.AspNetCore.Mvc;
using ThreatlockerAssetManagementSystem.DTOs.Auth;
using ThreatlockerAssetManagementSystem.Models.Entities;
using ThreatlockerAssetManagementSystem.Repositories;

namespace ThreatlockerAssetManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public AuthController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromForm] LoginDto loginData)
        {
            if (String.IsNullOrEmpty(loginData.EmailAddress)) return BadRequest("Email cannot be null or empty.");

            User? user = await _userRepository.GetByEmailAsync(loginData.EmailAddress);

            if (user == null) return BadRequest("User does not exist.");

            Response.Cookies.Append(
                "emailAddress",
                user.EmailAddress,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });

            return Ok();
        }

        [HttpGet("Me")]
        public async Task<ActionResult<User>> Me()
        {
            var emailAddress = Request.Cookies["emailAddress"];

            if (String.IsNullOrEmpty(emailAddress)) return Unauthorized();

            User? user = await _userRepository.GetByEmailAsync(emailAddress);

            if (user == null) return Unauthorized();

            return Ok(user);
        }
    }
}
