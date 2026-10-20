using System.Security.Claims;
using Rumos.ShopMate.Domain.Model;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.WebAPI.Security;

public class UserResolverService(IHttpContextAccessor accessor) : IUserResolverService
{
    private ClaimsPrincipal _user = accessor.HttpContext?.User ??
                                    throw new InvalidOperationException("There is no HTTP request");


    public int GetById()
    {
        if (_user.Identity?.IsAuthenticated != true
            || !int.TryParse(_user.FindFirst("sub")?.Value, out var userId)
            || userId <= 0)
        {
            throw new InvalidOperationException("There is no authenticated user");
        }

        return userId;
    }

    public string? GetFullName() => _user.FindFirst("name")?.Value;

    public string? GetUserName() => _user.FindFirst("username")?.Value;
    
    public string? GetRole() => _user.FindFirst("role")?.Value;
}
