namespace Rumos.ShopMate.Domain.Model;

public class Category
{
    public string Value { get; set; }

    public Category(string name)
    {
        Value = name;
    }
}