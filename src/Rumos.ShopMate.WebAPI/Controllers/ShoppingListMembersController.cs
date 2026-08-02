using Microsoft.AspNetCore.Mvc;
using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Exceptions;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.WebAPI.Controllers;

[Route("api/shopping-lists/{listId:int}/members")]
public class ShoppingListMembersController(IShoppingListService service) : BaseController
{
    // TODO: Move repeated exception handling to middleware
    // when exception middleware is introduced.
    // TODO: Read the current user ID from the authenticated identity
    // after JWT authentication is introduced.
    [HttpPost]
    public ActionResult<ShoppingListDto> Create(
        int listId,
        int userId,
        ShareShoppingListDto memberDto)
    {
        try
        {
            var shoppingList = service.Share(
                listId,
                memberDto.Username,
                memberDto.Role,
                userId);

            return CreatedAtRoute(
                "GetShoppingListById",
                new { listId, userId },
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

    [HttpPut("{memberUserId:int}")]
    public ActionResult<ShoppingListDto> ChangeRole(
        int listId,
        int memberUserId,
        int userId,
        UpdateShoppingListMemberDto memberDto)
    {
        try
        {
            return Ok(service.ChangeMemberRole(listId, memberUserId, memberDto, userId));
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

    [HttpDelete("{memberUserId:int}")]
    public IActionResult Remove(int listId, int memberUserId, int userId)
    {
        try
        {
            service.RemoveMember(listId, memberUserId, userId);
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
