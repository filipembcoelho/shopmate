using Microsoft.AspNetCore.Mvc;
using Rumos.ShopMate.Domain.Model;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.WebAPI.Controllers;

public class ShoppingListController(IShoppingListService service) : BaseController
{
    [HttpGet]
    public ActionResult<ShoppingListDto> GetShoppingListByUserId()
    {
        return Ok(service.GetForUser(1));
    }

    [HttpPost]
    public ActionResult<ShoppingListDto> CreateShoppingList(CreateShoppingListDto shoppingListDto)
    {
        var createdList = service.Create(shoppingListDto);
        return CreatedAtAction(nameof(GetShoppingListByUserId), new { userId = createdList.Owner.Id }, createdList);
    }
}

// Thin vs fat controllers