using Rumos.ShopMate.Domain.Model.Enums;

namespace Rumos.ShopMate.Services.Dtos;

public class CreateShoppingListItemDto
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Unit Unit { get; set; }
}
