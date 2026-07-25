using Rumos.ShopMate.ConsoleApp.Ui;
using Rumos.ShopMate.Domain.Model.Enums;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.ConsoleApp.Menus;

public class ShoppingModeMenu(
    ConsoleUi ui,
    IShoppingListService shoppingListService,
    ReceiptPrinter receiptPrinter)
{
    private int _currentItemIndex;

    public void Show(
        ShoppingListDto shoppingList,
        UserDto currentUser)
    {
        var activeShoppingList = shoppingList;
        var finish = false;
        _currentItemIndex = 0;

        while (!finish)
        {
            ui.Clear();
            ui.ShowTitle(
                "SHOPPING MISSION - " + activeShoppingList.Name);
            ui.WriteProgressBar(activeShoppingList);
            Console.WriteLine();

            var item = GetCurrentPendingItem(activeShoppingList);

            if (item == null)
            {
                ui.ShowMessage("All items are completed.");
                receiptPrinter.Print(activeShoppingList);
                ui.Pause();
                return;
            }

            Console.WriteLine("Current item:");
            Console.WriteLine(
                item.Name + " | " +
                item.Quantity + " " +
                item.Unit + " | " +
                item.Category);
            Console.WriteLine();
            ui.WriteMenuOption("1", "Mark as found");
            ui.WriteMenuOption("2", "Skip item");
            ui.WriteMenuOption("3", "Add surprise item");
            ui.WriteMenuOption("4", "Finish and print receipt");
            ui.WriteMenuOption("0", "Return to user menu");
            Console.WriteLine();

            var option = ui.AskText("Choose an option");

            switch (option)
            {
                case "1":
                    activeShoppingList = shoppingListService.CompleteItem(
                        activeShoppingList.Id,
                        item.Id,
                        currentUser.Id);
                    ui.ShowMessage("Found: " + item.Name);
                    ui.Pause();
                    break;
                case "2":
                    MoveToNextItem(activeShoppingList);
                    break;
                case "3":
                    activeShoppingList = AddSurpriseItem(
                        activeShoppingList,
                        currentUser);
                    ui.Pause();
                    break;
                case "4":
                    receiptPrinter.Print(activeShoppingList);
                    ui.Pause();
                    finish = true;
                    break;
                case "0":
                    finish = true;
                    break;
                default:
                    ui.ShowWarning("Invalid option.");
                    ui.Pause();
                    break;
            }
        }
    }

    private ShoppingListItemDto GetCurrentPendingItem(
        ShoppingListDto shoppingList)
    {
        if (shoppingList.Items.Count == 0)
        {
            return null;
        }

        for (var attempts = 0;
             attempts < shoppingList.Items.Count;
             attempts++)
        {
            if (_currentItemIndex >= shoppingList.Items.Count)
            {
                _currentItemIndex = 0;
            }

            var item = shoppingList.Items[_currentItemIndex];

            if (!item.IsCompleted)
            {
                return item;
            }

            _currentItemIndex++;
        }

        return null;
    }

    private void MoveToNextItem(ShoppingListDto shoppingList)
    {
        _currentItemIndex++;

        if (_currentItemIndex >= shoppingList.Items.Count)
        {
            _currentItemIndex = 0;
        }
    }

    private ShoppingListDto AddSurpriseItem(
        ShoppingListDto shoppingList,
        UserDto currentUser)
    {
        var itemName = "Chocolate";

        if (shoppingList.Items.Count % 2 == 0)
        {
            itemName = "Sparkling water";
        }

        var item = shoppingListService.AddItem(
            shoppingList.Id,
            itemName,
            1,
            Unit.Each,
            currentUser.Id);

        ui.ShowMessage("Surprise item added: " + item.Name);
        ui.ShowMessage("Auto category: " + item.Category);

        return shoppingListService.GetById(
            shoppingList.Id,
            currentUser.Id);
    }
}
