using Microsoft.AspNetCore.Mvc;
using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Exceptions;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.WebAPI.Controllers;

[Route("api/auth")]
public class AuthenticationController(IAuthenticationService service) : BaseController
{
    // TODO: Move repeated exception handling to middleware
    // when exception middleware is introduced.
    [HttpPost("login")]
    public ActionResult<UserDto> Login(LoginDto loginDto)
    {
        try
        {
            var user = service.Login(loginDto.Username, loginDto.Password);
            return Ok(user);
        }
        catch (InvalidCredentialsException ex)
        {
            return Unauthorized(new { message = ex.Message });
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

    [HttpPost("register")]
    public ActionResult<UserDto> Register(RegisterUserDto registerUserDto)
    {
        try
        {
            var user = service.Register(registerUserDto);

            return CreatedAtAction(
                nameof(UsersController.GetByUsername),
                "Users",
                new { username = user.Username },
                user);
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
