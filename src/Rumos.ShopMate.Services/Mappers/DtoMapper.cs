using Rumos.ShopMate.Data;
using Rumos.ShopMate.Domain.Model;
using Rumos.ShopMate.Services.Dtos;

namespace Rumos.ShopMate.Services.Mappers;

internal static class DtoMapper
{
    public static UserDto ToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FullName = user.Name.ToString(),
            Username = user.Account.Username
        };
    }

    public static ShoppingListDto ToDto(ShoppingList shoppingList)
    {
        return new ShoppingListDto
        {
            Id = shoppingList.Id,
            Name = shoppingList.Name,
            Owner = ToDto(shoppingList.Owner),
            ExpireDate = shoppingList.ExpireDate,
            IsArchived = shoppingList.IsArchived,
            Members = shoppingList.Members
                .Select(ToDto)
                .ToList(),
            Items = shoppingList.Items
                .Select(ToDto)
                .ToList(),
            Activities = shoppingList.Activities
                .Select(ToDto)
                .ToList(),
            CompletedItems = shoppingList.CountCompletedItems(),
            PendingItems = shoppingList.CountPendingItems(),
            ProgressPercentage = shoppingList.GetProgressPercentage()
        };
    }

    public static ShoppingListItemDto ToDto(ShoppingListItem item)
    {
        return new ShoppingListItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Quantity = item.Quantity,
            Unit = item.Unit,
            Category = item.Category.Value,
            IsCompleted = item.IsCompleted
        };
    }

    public static ProductDto ToDto(ProductCatalogItem product)
    {
        return new ProductDto
        {
            Name = product.Name,
            Unit = product.Unit
        };
    }

    private static ShoppingListMemberDto ToDto(ShoppingListMember member)
    {
        return new ShoppingListMemberDto
        {
            UserId = member.UserId,
            FullName = member.User.Name.ToString(),
            Username = member.User.Account.Username,
            Role = member.Role
        };
    }

    private static ShoppingListActivityDto ToDto(Activity activity)
    {
        return new ShoppingListActivityDto
        {
            Id = activity.Id,
            Description = activity.Description,
            CreatedAt = activity.CreatedAt
        };
    }
}
