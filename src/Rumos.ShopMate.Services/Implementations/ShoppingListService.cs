using Microsoft.EntityFrameworkCore;
using Rumos.ShopMate.Data;
using Rumos.ShopMate.Domain.Model;
using Rumos.ShopMate.Domain.Model.Enums;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Exceptions;
using Rumos.ShopMate.Services.Interfaces;
using Rumos.ShopMate.Services.Mappers;

namespace Rumos.ShopMate.Services.Implementations;

public class ShoppingListService(ApplicationContext context) : IShoppingListService
{
    public IReadOnlyList<ShoppingListDto> GetForUser(int userId)
    {
        ValidatePositiveId(userId, "User ID must be greater than zero.");

        if (!context.Users.Any(user => user.Id == userId))
        {
            throw new ServiceException("User was not found.");
        }

        return FullShoppingListQuery()
            .AsNoTracking()
            .Where(shoppingList =>
                shoppingList.Members.Any(member => member.UserId == userId))
            .OrderBy(shoppingList => shoppingList.Name)
            .ToList()
            .Select(DtoMapper.ToDto)
            .ToList();
    }

    public ShoppingListDto GetById(int shoppingListId, int userId)
    {
        ValidatePositiveId(
            shoppingListId,
            "Shopping list ID must be greater than zero.");
        ValidatePositiveId(userId, "User ID must be greater than zero.");

        var shoppingList = FullShoppingListQuery()
            .AsNoTracking()
            .SingleOrDefault(list =>
                list.Id == shoppingListId &&
                list.Members.Any(member => member.UserId == userId));

        if (shoppingList == null)
        {
            throw new ServiceException("Shopping list was not found.");
        }

        return DtoMapper.ToDto(shoppingList);
    }

    public ShoppingListDto Create(string name, int ownerId)
    {
        ValidateRequired(name, "Shopping list name is required.");
        ValidatePositiveId(ownerId, "Owner ID must be greater than zero.");

        var owner = context.Users
            .Include(user => user.Account)
            .SingleOrDefault(user => user.Id == ownerId);

        if (owner == null)
        {
            throw new ServiceException("Owner was not found.");
        }

        var shoppingList = new ShoppingList(name, owner);

        context.ShoppingLists.Add(shoppingList);
        context.SaveChanges();

        return DtoMapper.ToDto(shoppingList);
    }

    public ShoppingListItemDto AddItem(
        int shoppingListId,
        string name,
        int quantity,
        Unit unit,
        int userId)
    {
        ValidateRequired(name, "Item name is required.");

        if (quantity <= 0)
        {
            throw new ServiceException("Quantity must be greater than zero.");
        }

        if (!Enum.IsDefined(unit))
        {
            throw new ServiceException("Unit is invalid.");
        }

        var shoppingList = GetTrackedShoppingList(shoppingListId, userId);
        var requestedBy = GetRequestedBy(shoppingList, userId);
        var item = shoppingList.AddItem(name, quantity, unit, requestedBy);

        context.SaveChanges();

        return DtoMapper.ToDto(item);
    }

    public ShoppingListDto CompleteItem(
        int shoppingListId,
        int itemId,
        int userId)
    {
        ValidatePositiveId(
            itemId,
            "Shopping list item ID must be greater than zero.");

        var shoppingList = GetTrackedShoppingList(shoppingListId, userId);
        var item = shoppingList.Items
            .SingleOrDefault(existingItem => existingItem.Id == itemId);

        if (item == null)
        {
            throw new ServiceException("Shopping list item was not found.");
        }

        var requestedBy = GetRequestedBy(shoppingList, userId);
        shoppingList.CompleteItem(item, requestedBy);
        context.SaveChanges();

        return DtoMapper.ToDto(shoppingList);
    }

    public ShoppingListDto Share(
        int shoppingListId,
        string username,
        ShoppingListRole role,
        int userId)
    {
        ValidateRequired(username, "Username is required.");

        if (!Enum.IsDefined(role))
        {
            throw new ServiceException("Shopping list role is invalid.");
        }

        var shoppingList = GetTrackedShoppingList(shoppingListId, userId);
        var normalizedUsername = username.Trim().ToLower();
        var user = context.Users
            .Include(existingUser => existingUser.Account)
            .SingleOrDefault(existingUser =>
                existingUser.Account.Username == normalizedUsername);

        if (user == null)
        {
            throw new ServiceException("User was not found.");
        }

        var requestedBy = GetRequestedBy(shoppingList, userId);
        shoppingList.ShareWith(user, role, requestedBy);
        context.SaveChanges();

        return DtoMapper.ToDto(shoppingList);
    }

    public ShoppingListDto Archive(int shoppingListId, int userId)
    {
        var shoppingList = GetTrackedShoppingList(shoppingListId, userId);
        var requestedBy = GetRequestedBy(shoppingList, userId);

        shoppingList.Archive(requestedBy);
        context.SaveChanges();

        return DtoMapper.ToDto(shoppingList);
    }

    private ShoppingList GetTrackedShoppingList(int shoppingListId, int userId)
    {
        ValidatePositiveId(
            shoppingListId,
            "Shopping list ID must be greater than zero.");
        ValidatePositiveId(userId, "User ID must be greater than zero.");

        var shoppingList = FullShoppingListQuery()
            .SingleOrDefault(list =>
                list.Id == shoppingListId &&
                list.Members.Any(member => member.UserId == userId));

        if (shoppingList == null)
        {
            throw new ServiceException("Shopping list was not found.");
        }

        return shoppingList;
    }

    private IQueryable<ShoppingList> FullShoppingListQuery()
    {
        return context.ShoppingLists
            .Include(shoppingList => shoppingList.Owner)
                .ThenInclude(owner => owner.Account)
            .Include(shoppingList => shoppingList.Members)
                .ThenInclude(member => member.User)
                    .ThenInclude(user => user.Account)
            .Include(shoppingList => shoppingList.Items)
                .ThenInclude(item => item.Category)
            .Include(shoppingList => shoppingList.Activities);
    }

    private static User GetRequestedBy(ShoppingList shoppingList, int userId)
    {
        return shoppingList.Members
            .Single(member => member.UserId == userId)
            .User;
    }

    private static void ValidatePositiveId(int id, string message)
    {
        if (id <= 0)
        {
            throw new ServiceException(message);
        }
    }

    private static void ValidateRequired(string value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ServiceException(message);
        }
    }
}
