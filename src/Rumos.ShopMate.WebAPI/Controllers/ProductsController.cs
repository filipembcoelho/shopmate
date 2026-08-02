using Microsoft.AspNetCore.Mvc;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Exceptions;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.WebAPI.Controllers;

[Route("api/products")]
public class ProductsController(IProductCatalogService service) : BaseController
{
    // TODO: Move repeated exception handling to middleware
    // when exception middleware is introduced.
    [HttpGet]
    public ActionResult<IReadOnlyList<ProductDto>> Search(string searchText)
    {
        try
        {
            return Ok(service.Search(searchText));
        }
        catch (ServiceException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
