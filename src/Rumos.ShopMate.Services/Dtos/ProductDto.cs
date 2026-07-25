using Rumos.ShopMate.Domain.Model.Enums;

namespace Rumos.ShopMate.Services.Dtos;

public class ProductDto
{
    public string Name { get; set; } = string.Empty;
    public Unit Unit { get; set; }
}
