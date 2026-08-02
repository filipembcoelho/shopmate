namespace Rumos.ShopMate.Services.Dtos;

public class CreateShoppingListDto
{
    public string Name { get; set; } = string.Empty;
    public int OwnerId { get; set; }
}
