namespace Rumos.ShopMate.Services.Dtos;

public class ShoppingListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public UserDto Owner { get; set; } = new UserDto();
    public DateTime ExpireDate { get; set; }
    public bool IsArchived { get; set; }
    public List<ShoppingListMemberDto> Members { get; set; } = new();
    public List<ShoppingListItemDto> Items { get; set; } = new();
    public List<ShoppingListActivityDto> Activities { get; set; } = new();
    public int CompletedItems { get; set; }
    public int PendingItems { get; set; }
    public int ProgressPercentage { get; set; }
}


public class CreateShoppingListDto
{
    public string Name { get; set; }
    public int OwnerId { get; set; }
}