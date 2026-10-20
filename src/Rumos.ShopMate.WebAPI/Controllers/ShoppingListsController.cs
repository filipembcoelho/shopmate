using Microsoft.AspNetCore.Mvc;
using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Exceptions;
using Rumos.ShopMate.Services.Interfaces;
using Rumos.ShopMate.WebAPI.Security;

namespace Rumos.ShopMate.WebAPI.Controllers;

[Route("api/shopping-lists")]
public class ShoppingListsController(IShoppingListService service, IUserResolverService currentUser)
    : BaseController
{
    // TODO: Move repeated exception handling to middleware
    // when exception middleware is introduced.
    [HttpGet]
    public ActionResult<IReadOnlyList<ShoppingListDto>> GetForUser()
    {
        try
        {
            var userId = currentUser.GetById();
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
    public ActionResult<ShoppingListDto> GetById([FromRoute] int listId)
    {
        try
        {
            var userId = currentUser.GetById();
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
    public ActionResult<ShoppingListDto> Create(CreateListRequest request)
    {
        try
        {
            var userId = currentUser.GetById();
            var shoppingList = service.Create(new CreateShoppingListDto
            {
                Name = request.Name,
                OwnerId = userId
            });

            return CreatedAtRoute(
                "GetShoppingListById",
                new { listId = shoppingList.Id },
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
        UpdateShoppingListDto shoppingListDto)
    {
        try
        {
            var userId = currentUser.GetById();
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
    public IActionResult Archive(int listId)
    {
        try
        {
            var userId = currentUser.GetById();
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
