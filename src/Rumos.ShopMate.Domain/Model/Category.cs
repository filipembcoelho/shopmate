using Rumos.ShopMate.Domain.Exceptions;

namespace Rumos.ShopMate.Domain.Model;

public class Category
{
    public string Value { get; set; }

    public Category(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidShoppingListItemException("Category name is required.");
        }

        if (name.Trim().Length < 2)
        {
            throw new InvalidShoppingListItemException("Category name must be at least 2 characters long.");
        }

        if (name.Trim().Length > 30)
        {
            throw new InvalidShoppingListItemException("Category name must be at most 30 characters long.");
        }

        Value = name.Trim();
    }

    public override string ToString()
    {
        return Value;
    }
}
