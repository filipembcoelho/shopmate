using Rumos.ShopMate.ConsoleApp.Ui;
using Rumos.ShopMate.Domain.Model;
using Rumos.ShopMate.Domain.Model.Enums;

namespace Rumos.ShopMate.ConsoleApp.Menus;

public class ShoppingModeMenu(ConsoleUi ui, ShoppingList shoppingList, User currentUser)
{
    private int _currentItemIndex;

    public void Show()
    {
        var finish = false;

        while (!finish)
        {
            ui.Clear();
            ui.ShowTitle("SHOPPING MISSION - " + shoppingList.Name);
            ui.WriteProgressBar(shoppingList);
            Console.WriteLine();

            var item = GetCurrentPendingItem();

            if (item == null)
            {
                ui.ShowMessage("All items are completed.");
                var receiptPrinter = new ReceiptPrinter(ui);
                receiptPrinter.Print(shoppingList);
                ui.Pause();
                return;
            }

            Console.WriteLine("Current item:");
            Console.WriteLine(item.Name + " | " + item.Quantity + " " + item.Unit + " | " + item.Category.Value);
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
                    shoppingList.CompleteItem(item, currentUser);
                    ui.ShowMessage("Found: " + item.Name);
                    ui.Pause();
                    break;
                case "2":
                    MoveToNextItem();
                    break;
                case "3":
                    AddSurpriseItem();
                    ui.Pause();
                    break;
                case "4":
                    var receiptPrinter = new ReceiptPrinter(ui);
                    receiptPrinter.Print(shoppingList);
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

    private ShoppingListItem GetCurrentPendingItem()
    {
        if (shoppingList.Items.Count == 0)
        {
            return null;
        }

        for (var attempts = 0; attempts < shoppingList.Items.Count; attempts++)
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

    private void MoveToNextItem()
    {
        _currentItemIndex++;

        if (_currentItemIndex >= shoppingList.Items.Count)
        {
            _currentItemIndex = 0;
        }
    }

    private void AddSurpriseItem()
    {
        var itemName = "Chocolate";

        if (shoppingList.Items.Count % 2 == 0)
        {
            itemName = "Sparkling water";
        }

        var item = shoppingList.AddItem(itemName, 1, Unit.Each, currentUser);

        ui.ShowMessage("Surprise item added: " + item.Name);
        ui.ShowMessage("Auto category: " + item.Category.Value);
    }
}
