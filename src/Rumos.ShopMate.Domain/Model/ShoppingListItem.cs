using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Domain.Model.Common;
using Rumos.ShopMate.Domain.Model.Enums;

namespace Rumos.ShopMate.Domain.Model;

public class ShoppingListItem : AuditableEntity
{
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
    public int Quantity { get; set; }
    public Unit Unit { get; set; }
    public Category Category { get; set; }

    internal ShoppingListItem(string name, int quantity, Unit unit)
    {
        ValidateName(name);
        ValidateQuantity(quantity);
        ValidateUnit(unit);

        Name = name.Trim();
        Quantity = quantity;
        Unit = unit;
        IsCompleted = false;
    }

    internal ShoppingListItem(string name, int quantity, Unit unit, Category category) : this(name, quantity, unit)
    {
        ValidateCategory(category);
        Category = category;
    }

    internal void ChangeName(string name)
    {
        ValidateName(name);
        Name = name.Trim();
    }

    internal void ChangeQuantity(int quantity, Unit unit)
    {
        ValidateQuantity(quantity);
        ValidateUnit(unit);
        Quantity = quantity;
        Unit = unit;
    }

    internal void ChangeCategory(Category category)
    {
        ValidateCategory(category);
        Category = category;
    }

    internal void MarkAsCompleted()
    {
        IsCompleted = true;
    }

    internal void MarkAsPending()
    {
        IsCompleted = false;
    }

    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidShoppingListItemException("Item name is required.");
        }

        if (name.Trim().Length <= 1)
        {
            throw new InvalidShoppingListItemException("Item name must be at least 2 characters long.");
        }

        if (name.Trim().Length > 50)
        {
            throw new InvalidShoppingListItemException("Item name must be at most 50 characters long.");
        }
    }

    private void ValidateQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new InvalidShoppingListItemException("Quantity must be greater than zero.");
        }
    }

    private void ValidateUnit(Unit unit)
    {
        if (!Enum.IsDefined(typeof(Unit), unit))
        {
            throw new InvalidShoppingListItemException("Invalid unit.");
        }
    }

    private void ValidateCategory(Category category)
    {
        if (category == null)
        {
            throw new InvalidShoppingListItemException("Category is required.");
        }
    }
}
