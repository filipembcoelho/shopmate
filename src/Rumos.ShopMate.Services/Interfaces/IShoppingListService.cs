using Rumos.ShopMate.Domain.Model.Enums;
using Rumos.ShopMate.Services.Dtos;

namespace Rumos.ShopMate.Services.Interfaces;

public interface IShoppingListService
{
    IReadOnlyList<ShoppingListDto> GetForUser(int userId);
    ShoppingListDto GetById(int shoppingListId, int userId);
    ShoppingListDto Create(string name, int ownerId);
    ShoppingListItemDto AddItem(
        int shoppingListId,
        string name,
        int quantity,
        Unit unit,
        int userId);
    ShoppingListDto CompleteItem(int shoppingListId, int itemId, int userId);
    ShoppingListDto Share(
        int shoppingListId,
        string username,
        ShoppingListRole role,
        int userId);
    ShoppingListDto Archive(int shoppingListId, int userId);

    // TODO: Add an update operation when we implement PUT in the API.
    // TODO: Decide whether DELETE should remove a list or archive it.
}
