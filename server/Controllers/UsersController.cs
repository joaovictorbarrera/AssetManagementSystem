using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ThreatlockerAssetManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser()
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