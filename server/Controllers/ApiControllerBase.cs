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
                ServiceErrorType.NotFound => NotFound(),
                ServiceErrorType.Unauthorized => Unauthorized(),
                ServiceErrorType.Forbidden => StatusCode(StatusCodes.Status403Forbidden, new
                {
                    Title = result.ErrorMessage
                }),
                ServiceErrorType.BadRequest => BadRequest(new
                {
                    Title = result.ErrorMessage
                }),
                _ => StatusCode(500, new
                {
                    Title = "An unexpected error occurred"
                })
            };

        protected ActionResult ToActionResult<T>(ServiceResult<T> result) =>
            ToActionResult((ServiceResult)result);
    }
}