using Rumos.ShopMate.ConsoleApp.Ui;
using Rumos.ShopMate.Data;
using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.ConsoleApp.Components;

public class ListPicker(ConsoleUi ui)
{
    public ShoppingList ChooseShoppingList(User currentUser)
    {
        var shoppingLists = Database.GetShoppingListsFor(currentUser);

        if (shoppingLists.Count == 0)
        {
            ui.ShowWarning("You do not have any shopping lists yet.");
            return null;
        }

        Console.WriteLine("Choose a shopping list:");

        for (var i = 0; i < shoppingLists.Count; i++)
        {
            Console.WriteLine((i + 1) + ". " + shoppingLists[i].Name);
        }

        var option = ui.AskNumber("Number");

        if (option < 1 || option > shoppingLists.Count)
        {
            ui.ShowWarning("Invalid shopping list.");
            return null;
        }

        return shoppingLists[option - 1];
    }

    public ShoppingListItem ChooseItem(ShoppingList shoppingList)
    {
        if (shoppingList.Items.Count == 0)
        {
            ui.ShowWarning("This list has no items.");
            return null;
        }

        Console.WriteLine("Choose an item:");

        for (var i = 0; i < shoppingList.Items.Count; i++)
        {
            var item = shoppingList.Items[i];
            Console.WriteLine((i + 1) + ". " + item.Name);
        }

        var option = ui.AskNumber("Number");

        if (option < 1 || option > shoppingList.Items.Count)
        {
            ui.ShowWarning("Invalid item.");
            return null;
        }

        return shoppingList.Items[option - 1];
    }
}
