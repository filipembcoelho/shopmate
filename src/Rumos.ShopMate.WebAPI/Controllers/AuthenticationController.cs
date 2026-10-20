using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rumos.ShopMate.Domain.Constants;
using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Exceptions;
using Rumos.ShopMate.Services.Interfaces;
using Rumos.ShopMate.WebAPI.Security;

namespace Rumos.ShopMate.WebAPI.Controllers;

[Route("api/auth")]
public class AuthenticationController(
    IAuthenticationService service,
    ITokenService tokenService,
    IUserResolverService userResolverService) : BaseController
{
    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult<LoginResponseDto> Login(LoginDto loginDto)
    {
        try
        {
            var user = service.Login(loginDto.Username, loginDto.Password);
            var tokenDto = tokenService.GenerateToken(user);

            return Ok(new LoginResponseDto()
            {
                User = user,
                AccessToken = tokenDto.AccessToken,
                ExpiresAtUtc = tokenDto.ExpiresAtUtc,
            });
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
    [AllowAnonymous]
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

    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            id = userResolverService.GetById(),
            fullName = userResolverService.GetFullName(),
            username = userResolverService.GetUserName(),
            role = userResolverService.GetRole(),
        });
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpGet("admin-check")]
    public IActionResult AdminCheck()
    {
        return Ok(new { message = "The API accepted the admin role" });
    }

}
