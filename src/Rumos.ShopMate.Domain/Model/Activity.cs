using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Domain.Model.Common;

namespace Rumos.ShopMate.Domain.Model;

public class Activity : AuditableEntity
{
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int ShoppingListId { get; set; }
    public ShoppingList ShoppingList { get; set; }

    // EF
    private Activity()
    {
    }

    public Activity(string description) : this()
    {
        Description = ValidateDescription(description);
    }

    private static string ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidShoppingListException("Activity description is required.");
        }

        if (description.Trim().Length > 200)
        {
            throw new InvalidShoppingListException("Activity description must be at most 200 characters long.");
        }

        return description.Trim();
    }
}