using Rumos.ShopMate.ConsoleApp.Ui;
using Rumos.ShopMate.Data;
using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Domain.Model;
using Rumos.ShopMate.Domain.Utils;

namespace Rumos.ShopMate.ConsoleApp.Menus;

public class MainMenu(ConsoleUi ui)
{
    public void Show()
    {
        Database.SeedData();

        var exit = false;

        while (!exit)
        {
            ui.Clear();
            ui.ShowTitle("MAIN MENU");
            ui.WriteMenuOption("1", "Login");
            ui.WriteMenuOption("2", "Register");
            ui.WriteMenuOption("0", "Exit");
            Console.WriteLine();

            var option = ui.AskText("Choose an option");

            switch (option)
            {
                case "1":
                    Login();
                    break;
                case "2":
                    Register();
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    ui.ShowWarning("Invalid option.");
                    ui.Pause();
                    break;
            }
        }

        ui.Clear();
        ui.ShowTitle("GOODBYE");
        ui.ShowMessage("Thank you for using ShopMate.");
    }

    private void Login()
    {
        ui.Clear();
        ui.ShowTitle("LOGIN");

        var username = ui.AskText("Username");
        var password = ui.AskPassword("Password");

        var user = Database.Login(username, password);

        if (user == null)
        {
            ui.ShowError("Invalid username or password.");
            ui.Pause();
            return;
        }

        ui.ShowMessage("Welcome, " + user.Name + ".");
        ui.Pause();

        var userMenu = new UserMenu(ui, user);
        userMenu.Show();
    }

    private void Register()
    {
        ui.Clear();
        ui.ShowTitle("REGISTER");

        try
        {
            var fullName = ui.AskText("Full name");
            var suggestedUsername = UsernameUtils.SuggestUsername(fullName, Database.Users);

            ui.ShowMessage("Suggested username: " + suggestedUsername);
            var username = ui.AskText("Username (press Enter to use suggestion)");

            if (string.IsNullOrWhiteSpace(username))
            {
                username = suggestedUsername;
            }

            Console.WriteLine();
            ui.ShowMessage("Password rules:");
            Console.WriteLine("- At least 8 characters");
            Console.WriteLine("- At least one capital letter");
            Console.WriteLine("- At least one lowercase letter");
            Console.WriteLine("- At least one number");
            Console.WriteLine("- At least one special character");
            Console.WriteLine();

            var password = ui.AskPassword("Password");
            var passwordConfirmation = ui.AskPassword("Confirm password");

            if (password != passwordConfirmation)
            {
                ui.ShowError("Passwords do not match.");
                ui.Pause();
                return;
            }

            var user = Database.RegisterUser(fullName, username, password);

            ui.ShowMessage("User created: " + user);
        }
        catch (DomainException ex)
        {
            ui.ShowDomainRule(ex.Message);
        }

        ui.Pause();
    }
}
