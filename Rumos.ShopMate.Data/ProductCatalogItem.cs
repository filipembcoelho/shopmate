using Rumos.ShopMate.Domain.Model.Enums;

namespace Rumos.ShopMate.Data;

public class ProductCatalogItem(string name, Unit unit)
{
    public string Name { get; set; } = name;
    public Unit Unit { get; set; } = unit;
}
