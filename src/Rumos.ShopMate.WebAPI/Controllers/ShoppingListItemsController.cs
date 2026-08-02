using Microsoft.AspNetCore.Mvc;
using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Exceptions;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.WebAPI.Controllers;

[Route("api/shopping-lists/{listId:int}/items")]
public class ShoppingListItemsController(IShoppingListService service) : BaseController
{
    // TODO: Move repeated exception handling to middleware
    // when exception middleware is introduced.
    // TODO: Read the current user ID from the authenticated identity
    // after JWT authentication is introduced.
    [HttpPost]
    public ActionResult<ShoppingListItemDto> Create(
        int listId,
        int userId,
        CreateShoppingListItemDto itemDto)
    {
        try
        {
            var item = service.AddItem(
                listId,
                itemDto.Name,
                itemDto.Quantity,
                itemDto.Unit,
                userId);

            return CreatedAtRoute(
                "GetShoppingListById",
                new { listId, userId },
                item);
        }
        catch (ServiceNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ServiceException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{itemId:int}")]
    public ActionResult<ShoppingListItemDto> Update(
        int listId,
        int itemId,
        int userId,
        UpdateShoppingListItemDto itemDto)
    {
        try
        {
            return Ok(service.UpdateItem(listId, itemId, itemDto, userId));
        }
        catch (ServiceNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ServiceException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{itemId:int}")]
    public IActionResult Remove(int listId, int itemId, int userId)
    {
        try
        {
            service.RemoveItem(listId, itemId, userId);
            return NoContent();
        }
        catch (ServiceNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ServiceException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
