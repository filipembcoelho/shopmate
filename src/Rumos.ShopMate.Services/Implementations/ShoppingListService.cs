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
            throw new ServiceNotFoundException("User was not found.");
        }

        // TODO: Consider EF Core query filters and interceptors for soft-deleted
        // shopping lists when these advanced persistence concepts are introduced.
        return FullShoppingListQuery()
            .AsNoTracking()
            .Where(shoppingList =>
                shoppingList.IsArchived == false &&
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
                list.IsArchived == false &&
                list.Members.Any(member => member.UserId == userId));

        if (shoppingList == null)
        {
            throw new ServiceNotFoundException("Shopping list was not found.");
        }

        return DtoMapper.ToDto(shoppingList);
    }

    public ShoppingListDto Create(CreateShoppingListDto  shoppingListDto)
    {
        ValidateRequired(shoppingListDto.Name, "Shopping list name is required.");
        ValidatePositiveId(shoppingListDto.OwnerId, "Owner ID must be greater than zero.");

        var owner = context.Users
            .Include(user => user.Account)
            .SingleOrDefault(user => user.Id == shoppingListDto.OwnerId);

        if (owner == null)
        {
            throw new ServiceNotFoundException("Owner was not found.");
        }

        var shoppingList = new ShoppingList(shoppingListDto.Name, owner);

        context.ShoppingLists.Add(shoppingList);
        context.SaveChanges();

        return DtoMapper.ToDto(shoppingList);
    }

    public ShoppingListDto Rename(
        int shoppingListId,
        UpdateShoppingListDto shoppingListDto,
        int userId)
    {
        if (shoppingListDto == null)
        {
            throw new ServiceException("Shopping list data is required.");
        }

        var shoppingList = GetTrackedShoppingList(shoppingListId, userId);
        var requestedBy = GetRequestedBy(shoppingList, userId);

        shoppingList.Rename(shoppingListDto.Name, requestedBy);
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
        var shoppingList = GetTrackedShoppingList(shoppingListId, userId);
        var item = GetItem(shoppingList, itemId);
        var requestedBy = GetRequestedBy(shoppingList, userId);
        shoppingList.CompleteItem(item, requestedBy);
        context.SaveChanges();

        return DtoMapper.ToDto(shoppingList);
    }

    public ShoppingListItemDto UpdateItem(
        int shoppingListId,
        int itemId,
        UpdateShoppingListItemDto itemDto,
        int userId)
    {
        if (itemDto == null)
        {
            throw new ServiceException("Shopping list item data is required.");
        }

        ValidateRequired(itemDto.Name, "Item name is required.");

        if (itemDto.Quantity <= 0)
        {
            throw new ServiceException("Quantity must be greater than zero.");
        }

        if (!Enum.IsDefined(itemDto.Unit))
        {
            throw new ServiceException("Unit is invalid.");
        }

        var shoppingList = GetTrackedShoppingList(shoppingListId, userId);
        var item = GetItem(shoppingList, itemId);
        var requestedBy = GetRequestedBy(shoppingList, userId);
        var trimmedName = itemDto.Name.Trim();

        if (item.Name != trimmedName)
        {
            shoppingList.ChangeItemName(item, itemDto.Name, requestedBy);
        }

        if (item.Quantity != itemDto.Quantity || item.Unit != itemDto.Unit)
        {
            shoppingList.ChangeItemQuantity(
                item,
                itemDto.Quantity,
                itemDto.Unit,
                requestedBy);
        }

        if (item.IsCompleted != itemDto.IsCompleted)
        {
            if (itemDto.IsCompleted)
            {
                shoppingList.CompleteItem(item, requestedBy);
            }
            else
            {
                shoppingList.ReopenItem(item, requestedBy);
            }
        }

        context.SaveChanges();

        return DtoMapper.ToDto(item);
    }

    public void RemoveItem(int shoppingListId, int itemId, int userId)
    {
        var shoppingList = GetTrackedShoppingList(shoppingListId, userId);
        var item = GetItem(shoppingList, itemId);
        var requestedBy = GetRequestedBy(shoppingList, userId);

        shoppingList.RemoveItem(item, requestedBy);
        context.SaveChanges();
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
            throw new ServiceNotFoundException("User was not found.");
        }

        var requestedBy = GetRequestedBy(shoppingList, userId);
        shoppingList.ShareWith(user, role, requestedBy);
        context.SaveChanges();

        return DtoMapper.ToDto(shoppingList);
    }

    public ShoppingListDto ChangeMemberRole(
        int shoppingListId,
        int memberUserId,
        UpdateShoppingListMemberDto memberDto,
        int userId)
    {
        if (memberDto == null)
        {
            throw new ServiceException("Shopping list member data is required.");
        }

        if (!Enum.IsDefined(memberDto.Role))
        {
            throw new ServiceException("Shopping list role is invalid.");
        }

        var shoppingList = GetTrackedShoppingList(shoppingListId, userId);
        var member = GetMember(shoppingList, memberUserId);
        var requestedBy = GetRequestedBy(shoppingList, userId);

        shoppingList.ChangeMemberRole(member.User, memberDto.Role, requestedBy);
        context.SaveChanges();

        return DtoMapper.ToDto(shoppingList);
    }

    public void RemoveMember(int shoppingListId, int memberUserId, int userId)
    {
        var shoppingList = GetTrackedShoppingList(shoppingListId, userId);
        var member = GetMember(shoppingList, memberUserId);
        var requestedBy = GetRequestedBy(shoppingList, userId);

        shoppingList.RemoveMember(member.User, requestedBy);
        context.SaveChanges();
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
                list.IsArchived == false &&
                list.Members.Any(member => member.UserId == userId));

        if (shoppingList == null)
        {
            throw new ServiceNotFoundException("Shopping list was not found.");
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

    private static ShoppingListItem GetItem(ShoppingList shoppingList, int itemId)
    {
        if (itemId <= 0)
        {
            throw new ServiceException("Shopping list item ID must be greater than zero.");
        }

        var item = shoppingList.Items
            .SingleOrDefault(existingItem => existingItem.Id == itemId);

        if (item == null)
        {
            throw new ServiceNotFoundException("Shopping list item was not found.");
        }

        return item;
    }

    private static ShoppingListMember GetMember(
        ShoppingList shoppingList,
        int memberUserId)
    {
        if (memberUserId <= 0)
        {
            throw new ServiceException("Member user ID must be greater than zero.");
        }

        var member = shoppingList.Members
            .SingleOrDefault(existingMember => existingMember.UserId == memberUserId);

        if (member == null)
        {
            throw new ServiceNotFoundException("Shopping list member was not found.");
        }

        return member;
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
