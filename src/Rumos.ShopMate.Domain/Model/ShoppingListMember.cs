using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Domain.Model.Common;
using Rumos.ShopMate.Domain.Model.Enums;

namespace Rumos.ShopMate.Domain.Model;

public class ShoppingListMember : Entity
{
    public User User { get; set; }
    public ShoppingListRole Role { get; set; }

    internal ShoppingListMember(User user, ShoppingListRole role)
    {
        if (user == null)
        {
            throw new InvalidUserException("User is required.");
        }

        ValidateRole(role);

        User = user;
        Role = role;
    }

    internal void ChangeRole(ShoppingListRole newRole)
    {
        ValidateRole(newRole);

        if (newRole == ShoppingListRole.Owner)
        {
            throw new PermissionDeniedException("Cannot promote another member to owner.");
        }

        Role = newRole;
    }

    private void ValidateRole(ShoppingListRole role)
    {
        if (!Enum.IsDefined(typeof(ShoppingListRole), role))
        {
            throw new InvalidShoppingListException("Invalid shopping list role.");
        }
    }
}
