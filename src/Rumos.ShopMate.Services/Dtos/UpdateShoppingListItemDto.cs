using Rumos.ShopMate.Domain.Model.Enums;

namespace Rumos.ShopMate.Services.Dtos;

public class UpdateShoppingListItemDto
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Unit Unit { get; set; }
    public bool IsCompleted { get; set; }
}
