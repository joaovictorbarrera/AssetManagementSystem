using AssetManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected ActionResult ToActionResult(ServiceResult result) =>
            result.ErrorType switch
            {
                ServiceErrorType.NotFound => NotFound(result.ErrorMessage),
                ServiceErrorType.Forbidden => Forbid(result.ErrorMessage ?? string.Empty),
                ServiceErrorType.BadRequest => BadRequest(result.ErrorMessage),
                _ => StatusCode(500, "An unexpected error occurred")
            };

        protected ActionResult ToActionResult<T>(ServiceResult<T> result) =>
            result.ErrorType switch
            {
                ServiceErrorType.NotFound => NotFound(result.ErrorMessage),
                ServiceErrorType.Forbidden => Forbid(result.ErrorMessage ?? string.Empty),
                ServiceErrorType.BadRequest => BadRequest(result.ErrorMessage),
                _ => StatusCode(500, "An unexpected error occurred")
            };
    }
}