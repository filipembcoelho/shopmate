namespace Rumos.ShopMate.Services.Dtos;

public class TokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
}