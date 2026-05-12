using BMedia.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BMedia.API.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    private ISender? _sender;
    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected IActionResult ToActionResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return result.StatusCode switch
            {
                201 => Created(string.Empty, result.Data),
                204 => NoContent(),
                _ => Ok(result.Data)
            };
        }

        return result.StatusCode switch
        {
            401 => Unauthorized(new { error = result.Error }),
            403 => Forbid(),
            404 => NotFound(new { error = result.Error }),
            409 => Conflict(new { error = result.Error }),
            _ => BadRequest(new { error = result.Error, errors = result.Errors })
        };
    }
}
