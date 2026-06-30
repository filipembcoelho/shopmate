using Rumos.ShopMate.Domain.Model.Enums;

namespace Rumos.ShopMate.Domain.Model;

public class ShoppingListMember(User user, ShoppingListRole role)
{
    public User User { get; private set; } = user;
    public ShoppingListRole Role { get; private set; } = role;

    public void ChangeRole(ShoppingListRole newRole)
    {
        Role = newRole;
    }
}