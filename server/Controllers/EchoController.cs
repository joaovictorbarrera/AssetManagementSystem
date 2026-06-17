using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ThreatlockerAssetManagementSystem.Controllers
{
    [ApiController]
    public class EchoController : ControllerBase
    {
        [HttpGet("/")]
        [HttpGet("/api")]
        public IActionResult Echo()
        {
            return Ok(new 
            { 
                Hostname = Environment.MachineName, 
                InboundIPAddress = HttpContext.Connection.RemoteIpAddress?.ToString() 
            });
        }
    }
}
