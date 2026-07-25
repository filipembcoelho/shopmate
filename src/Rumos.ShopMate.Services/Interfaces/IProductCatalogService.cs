using Rumos.ShopMate.Services.Dtos;

namespace Rumos.ShopMate.Services.Interfaces;

public interface IProductCatalogService
{
    IReadOnlyList<ProductDto> Search(string searchText);
}
