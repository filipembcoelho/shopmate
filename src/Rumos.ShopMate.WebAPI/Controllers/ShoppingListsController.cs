using Microsoft.AspNetCore.Mvc;
using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Exceptions;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.WebAPI.Controllers;

[Route("api/shopping-lists")]
public class ShoppingListsController(IShoppingListService service) : BaseController
{
    // TODO: Move repeated exception handling to middleware
    // when exception middleware is introduced.
    // TODO: Read the current user ID from the authenticated identity
    // after JWT authentication is introduced.
    [HttpGet]
    public ActionResult<IReadOnlyList<ShoppingListDto>> GetForUser(int userId)
    {
        try
        {
            return Ok(service.GetForUser(userId));
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

    [HttpGet("{listId:int}", Name = "GetShoppingListById")]
    public ActionResult<ShoppingListDto> GetById(int listId, int userId)
    {
        try
        {
            return Ok(service.GetById(listId, userId));
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

    [HttpPost]
    public ActionResult<ShoppingListDto> Create(CreateShoppingListDto shoppingListDto)
    {
        try
        {
            var shoppingList = service.Create(shoppingListDto);

            return CreatedAtRoute(
                "GetShoppingListById",
                new { listId = shoppingList.Id, userId = shoppingList.Owner.Id },
                shoppingList);
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

    [HttpPut("{listId:int}")]
    public ActionResult<ShoppingListDto> Rename(
        int listId,
        int userId,
        UpdateShoppingListDto shoppingListDto)
    {
        try
        {
            return Ok(service.Rename(listId, shoppingListDto, userId));
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

    [HttpDelete("{listId:int}")]
    public IActionResult Archive(int listId, int userId)
    {
        try
        {
            service.Archive(listId, userId);
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
