using Rumos.ShopMate.Data;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Exceptions;
using Rumos.ShopMate.Services.Interfaces;
using Rumos.ShopMate.Services.Mappers;

namespace Rumos.ShopMate.Services.Implementations;

public class ProductCatalogService : IProductCatalogService
{
    public IReadOnlyList<ProductDto> Search(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            throw new ServiceException("Search text is required.");
        }

        return ProductCatalog.Search(searchText)
            .Select(DtoMapper.ToDto)
            .ToList();
    }
}
