using Rumos.ShopMate.Domain.Model;
using Rumos.ShopMate.Domain.Model.Enums;

namespace Rumos.ShopMate.ConsoleApp.Ui;

public class ConsoleUi
{
    public void Clear()
    {
        if (Console.IsOutputRedirected)
        {
            Console.WriteLine();
            Console.WriteLine();
            return;
        }

        Console.Clear();
    }

    public void ShowTitle(string title)
    {
        WriteLine("========================================", ConsoleColor.DarkCyan);
        WriteLine("           SHOPMATE TERMINAL            ", ConsoleColor.Cyan);
        WriteLine("========================================", ConsoleColor.DarkCyan);
        WriteLine(title, ConsoleColor.Yellow);
        WriteLine("----------------------------------------", ConsoleColor.DarkCyan);
        Console.WriteLine();
    }

    public void ShowMessage(string message)
    {
        WriteLine(message, ConsoleColor.Green);
    }

    public void ShowWarning(string message)
    {
        WriteLine(message, ConsoleColor.Yellow);
    }

    public void ShowError(string message)
    {
        WriteLine(message, ConsoleColor.Red);
    }

    public void ShowDomainRule(string message)
    {
        WriteLine("DOMAIN RULE: " + message, ConsoleColor.Magenta);
    }

    public string AskText(string label)
    {
        Write(label + ": ", ConsoleColor.Gray);
        var value = Console.ReadLine();

        if (value == null)
        {
            return "";
        }

        return value;
    }

    public string AskPassword(string label)
    {
        Write(label + ": ", ConsoleColor.Gray);

        if (Console.IsInputRedirected)
        {
            var redirectedPassword = Console.ReadLine();

            if (redirectedPassword == null)
            {
                return "";
            }

            return redirectedPassword;
        }

        var password = "";
        var finished = false;

        while (!finished)
        {
            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter)
            {
                finished = true;
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            }
            else
            {
                password = password + key.KeyChar;
                Console.Write("*");
            }
        }

        Console.WriteLine();
        return password;
    }

    public int AskNumber(string label)
    {
        Write(label + ": ", ConsoleColor.Gray);

        var value = Console.ReadLine();
        var isValid = int.TryParse(value, out var number);

        if (!isValid)
        {
            return 0;
        }

        return number;
    }

    public void Pause()
    {
        Console.WriteLine();
        Write("Press Enter to continue...", ConsoleColor.DarkGray);
        Console.ReadLine();
    }

    public void WriteMenuOption(string number, string text)
    {
        Write("[" + number + "] ", ConsoleColor.Cyan);
        Console.WriteLine(text);
    }

    public void WriteListHeader(ShoppingList shoppingList)
    {
        Console.WriteLine();
        WriteLine("+--------------------------------------+", ConsoleColor.DarkCyan);
        WriteLine("| " + shoppingList.Name, ConsoleColor.Cyan);
        WriteLine("+--------------------------------------+", ConsoleColor.DarkCyan);
        Console.WriteLine("Owner    : " + shoppingList.Owner.Name);
        Console.WriteLine("Archived : " + shoppingList.IsArchived);
        Console.WriteLine("Expires  : " + shoppingList.ExpireDate.ToShortDateString());
    }

    public void WriteMembers(ShoppingList shoppingList)
    {
        Console.WriteLine();
        WriteLine("Members", ConsoleColor.Yellow);

        foreach (ShoppingListMember member in shoppingList.Members)
        {
            Console.WriteLine("- " + member.User.Name + " [" + member.Role + "]");
        }
    }

    public void WriteItems(ShoppingList shoppingList)
    {
        Console.WriteLine();
        WriteLine("Items", ConsoleColor.Yellow);

        if (shoppingList.Items.Count == 0)
        {
            Console.WriteLine("- No items yet");
            return;
        }

        for (var i = 0; i < shoppingList.Items.Count; i++)
        {
            var item = shoppingList.Items[i];
            var status = "pending";

            if (item.IsCompleted)
            {
                status = "done";
            }

            Console.WriteLine(
                (i + 1) + ". " +
                item.Name + " | " +
                item.Quantity + " " +
                item.Unit + " | " +
                GetCategoryName(item) + " | " +
                status);
        }
    }

    public void WriteProgressBar(ShoppingList shoppingList)
    {
        var progress = shoppingList.GetProgressPercentage();
        var filledBlocks = progress / 10;
        var bar = "";

        for (var i = 0; i < 10; i++)
        {
            if (i < filledBlocks)
            {
                bar = bar + "#";
            }
            else
            {
                bar = bar + "-";
            }
        }

        Write("[", ConsoleColor.DarkGray);
        Write(bar, ConsoleColor.Green);
        Write("] ", ConsoleColor.DarkGray);
        Console.WriteLine(progress + "% (" + shoppingList.CountCompletedItems() + "/" + shoppingList.Items.Count + ")");
    }

    public void WriteActivities(ShoppingList shoppingList)
    {
        if (shoppingList.Activities.Count == 0)
        {
            Console.WriteLine("No activity yet.");
            return;
        }

        foreach (var activity in shoppingList.Activities)
        {
            Console.WriteLine("- " + activity.CreatedAt.ToShortTimeString() + " | " + activity.Description);
        }
    }

    public void WriteUnitOptions()
    {
        WriteMenuOption("1", "Each");
        WriteMenuOption("2", "Kilogram");
        WriteMenuOption("3", "Gram");
        WriteMenuOption("4", "Liter");
        WriteMenuOption("5", "Milliliter");
        WriteMenuOption("6", "Dozen");
        WriteMenuOption("7", "Package");
        WriteMenuOption("8", "Box");
        WriteMenuOption("9", "Bag");
        WriteMenuOption("10", "Bottle");
        WriteMenuOption("11", "Can");
        WriteMenuOption("12", "Jar");
        WriteMenuOption("13", "Roll");
        WriteMenuOption("14", "Bunch");
        WriteMenuOption("15", "Slice");
    }

    public void WriteRoleOptions()
    {
        WriteMenuOption("1", "Editor");
        WriteMenuOption("2", "Viewer");
    }

    public Unit GetUnitFromOption(int option)
    {
        switch (option)
        {
            case 1:
                return Unit.Each;
            case 2:
                return Unit.Kilogram;
            case 3:
                return Unit.Gram;
            case 4:
                return Unit.Liter;
            case 5:
                return Unit.Milliliter;
            case 6:
                return Unit.Dozen;
            case 7:
                return Unit.Package;
            case 8:
                return Unit.Box;
            case 9:
                return Unit.Bag;
            case 10:
                return Unit.Bottle;
            case 11:
                return Unit.Can;
            case 12:
                return Unit.Jar;
            case 13:
                return Unit.Roll;
            case 14:
                return Unit.Bunch;
            case 15:
                return Unit.Slice;
            default:
                ShowWarning("Invalid unit. Using Each.");
                return Unit.Each;
        }
    }

    public ShoppingListRole GetRoleFromOption(int option)
    {
        switch (option)
        {
            case 1:
                return ShoppingListRole.Editor;
            case 2:
                return ShoppingListRole.Viewer;
            default:
                ShowWarning("Invalid role. Using Viewer.");
                return ShoppingListRole.Viewer;
        }
    }

    private void Write(string text, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = originalColor;
    }

    private void WriteLine(string text, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = originalColor;
    }

    private string GetCategoryName(ShoppingListItem item)
    {
        if (item.Category == null)
        {
            return "Other";
        }

        return item.Category.Value;
    }
}
