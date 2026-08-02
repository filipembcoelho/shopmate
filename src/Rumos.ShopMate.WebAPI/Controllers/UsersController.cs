using Microsoft.AspNetCore.Mvc;
using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Exceptions;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.WebAPI.Controllers;

[Route("api/users")]
public class UsersController(IUserService service) : BaseController
{
    // TODO: Move repeated exception handling to middleware
    // when exception middleware is introduced.
    [HttpGet("suggest-username")]
    public ActionResult<string> SuggestUsername(string fullName)
    {
        try
        {
            return Ok(service.SuggestUsername(fullName));
        }
        catch (ServiceException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{username}")]
    public ActionResult<UserDto> GetByUsername(string username)
    {
        try
        {
            return Ok(service.GetByUsername(username));
        }
        catch (ServiceNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ServiceException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
