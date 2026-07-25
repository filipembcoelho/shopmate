using Rumos.ShopMate.Domain.Model.Enums;

namespace Rumos.ShopMate.Services.Dtos;

public class ShoppingListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Unit Unit { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}
