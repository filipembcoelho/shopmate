using Rumos.ShopMate.Domain.Model.Enums;
using Rumos.ShopMate.Services.Dtos;

namespace Rumos.ShopMate.Services.Interfaces;

public interface IShoppingListService
{
    IReadOnlyList<ShoppingListDto> GetForUser(int userId);
    ShoppingListDto GetById(int shoppingListId, int userId);
    ShoppingListDto Create(CreateShoppingListDto shoppingListDto);
    ShoppingListDto Rename(
        int shoppingListId,
        UpdateShoppingListDto shoppingListDto,
        int userId);
    ShoppingListItemDto AddItem(
        int shoppingListId,
        string name,
        int quantity,
        Unit unit,
        int userId);
    ShoppingListItemDto UpdateItem(
        int shoppingListId,
        int itemId,
        UpdateShoppingListItemDto itemDto,
        int userId);
    void RemoveItem(int shoppingListId, int itemId, int userId);
    ShoppingListDto CompleteItem(int shoppingListId, int itemId, int userId);
    ShoppingListDto Share(
        int shoppingListId,
        string username,
        ShoppingListRole role,
        int userId);
    ShoppingListDto ChangeMemberRole(
        int shoppingListId,
        int memberUserId,
        UpdateShoppingListMemberDto memberDto,
        int userId);
    void RemoveMember(int shoppingListId, int memberUserId, int userId);
    ShoppingListDto Archive(int shoppingListId, int userId);
}
