using Microsoft.AspNetCore.Mvc;
using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Exceptions;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.WebAPI.Controllers;

[Route("api/shopping-lists/{listId:int}/members")]
public class ShoppingListMembersController(IShoppingListService service, IUserResolverService currentUser) : BaseController
{
    // TODO: Move repeated exception handling to middleware
    // when exception middleware is introduced.
    [HttpPost]
    public ActionResult<ShoppingListDto> Create(
        int listId,
        ShareShoppingListDto memberDto)
    {
        try
        {
            var userId = currentUser.GetById();
            var shoppingList = service.Share(
                listId,
                memberDto.Username,
                memberDto.Role,
                userId);

            return CreatedAtRoute(
                "GetShoppingListById",
                new { listId },
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
        UpdateShoppingListMemberDto memberDto)
    {
        try
        {
            var userId = currentUser.GetById();
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
    public IActionResult Remove(int listId, int memberUserId)
    {
        try
        {
            var userId = currentUser.GetById();
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
