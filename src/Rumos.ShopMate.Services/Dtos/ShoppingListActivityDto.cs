namespace Rumos.ShopMate.Services.Dtos;

public class ShoppingListActivityDto
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
