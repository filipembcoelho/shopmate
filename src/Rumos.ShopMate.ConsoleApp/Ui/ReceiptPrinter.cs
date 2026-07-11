using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.ConsoleApp.Ui;

public class ReceiptPrinter(ConsoleUi ui)
{
    public void Print(ShoppingList shoppingList)
    {
        ui.Clear();
        ui.ShowTitle("SHOPMATE RECEIPT");

        Console.WriteLine("List: " + shoppingList.Name);
        Console.WriteLine("Owner: " + shoppingList.Owner.Name);
        Console.WriteLine();

        Console.WriteLine("Bought:");
        WriteItemsByCompletedState(shoppingList, true);

        Console.WriteLine();
        Console.WriteLine("Still missing:");
        WriteItemsByCompletedState(shoppingList, false);

        Console.WriteLine();
        Console.WriteLine("Score: " + shoppingList.CountCompletedItems() + " / " + shoppingList.Items.Count);
        ui.WriteProgressBar(shoppingList);
    }

    private void WriteItemsByCompletedState(ShoppingList shoppingList, bool completed)
    {
        var foundAny = false;

        foreach (var item in shoppingList.Items)
        {
            if (item.IsCompleted == completed)
            {
                Console.WriteLine("- " + item.Name + " | " + item.Quantity + " " + item.Unit + " | " + item.Category.Value);
                foundAny = true;
            }
        }

        if (!foundAny)
        {
            Console.WriteLine("- none");
        }
    }
}
