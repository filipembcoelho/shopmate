using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Domain.Model.Common;

namespace Rumos.ShopMate.Domain.Model;

public class Activity(string description) : AuditableEntity
{
    public string Description { get; set; } = ValidateDescription(description);
    public DateTime CreatedAt { get; set; } = DateTime.Now;

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
