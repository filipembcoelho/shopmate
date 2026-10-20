using Rumos.ShopMate.Services.Dtos;

namespace Rumos.ShopMate.WebAPI.Security;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
    public UserDto User { get; set; } = new();
}
