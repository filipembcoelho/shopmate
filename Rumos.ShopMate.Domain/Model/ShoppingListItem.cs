using Rumos.ShopMate.Domain.Model.Enums;

namespace Rumos.ShopMate.Domain.Model;

public class ShoppingListItem
{
    public string Name { get; private set; }
    public bool IsCompleted { get; set; }
    public int Quantity { get; set; }
    public Unit Unit { get; set; }
    public Category Category { get; set; }

    public ShoppingListItem(string name, int quantity, Unit unit)
    {
        ValidateName(name);
        Name = name;

        Quantity = quantity;
        Unit = unit;
    }

    public ShoppingListItem(string name, int quantity, Unit unit, Category category) : this(name, quantity, unit)
    {
        Category = category;
    }

    // Acções
    public void ChangeName(string name)
    {
        ValidateName(name);
        Name = name;
    }

    // new method
    private void ValidateName(string name)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (name.Length <= 1)
        {
            throw new ArgumentException("Name must be at least 2 characters long.");
        }

        if (name.Length > 50)
        {
            throw new ArgumentException("Name must be at most 50 characters long.");
        }
    }
}