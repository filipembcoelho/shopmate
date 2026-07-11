using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Data;

public class MemorySeedData()
{
    public List<User> Users { get; set; } = new List<User>();
    public List<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();
}
