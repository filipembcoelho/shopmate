using Rumos.ShopMate.Domain.Model.Enums;

namespace Rumos.ShopMate.Services.Dtos;

public class ShareShoppingListDto
{
    public string Username { get; set; } = string.Empty;
    public ShoppingListRole Role { get; set; }
}
