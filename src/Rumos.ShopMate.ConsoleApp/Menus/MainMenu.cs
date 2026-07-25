using Rumos.ShopMate.ConsoleApp.Ui;
using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Exceptions;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.ConsoleApp.Menus;

public class MainMenu(
    ConsoleUi ui,
    IAuthenticationService authenticationService,
    IUserService userService,
    UserMenu userMenu)
{
    public void Show()
    {
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

        try
        {
            var username = ui.AskText("Username");
            var password = ui.AskPassword("Password");
            var user = authenticationService.Login(username, password);

            if (user == null)
            {
                ui.ShowError("Invalid username or password.");
                ui.Pause();
                return;
            }

            ui.ShowMessage("Welcome, " + user.FullName + ".");
            ui.Pause();

            userMenu.Show(user);
        }
        catch (ServiceException ex)
        {
            ui.ShowError(ex.Message);
            ui.Pause();
        }
        catch (DomainException ex)
        {
            ui.ShowDomainRule(ex.Message);
            ui.Pause();
        }
    }

    private void Register()
    {
        ui.Clear();
        ui.ShowTitle("REGISTER");

        try
        {
            var fullName = ui.AskText("Full name");
            var suggestedUsername = userService.SuggestUsername(fullName);

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

            var user = authenticationService.Register(new RegisterUserDto
            {
                FullName = fullName,
                Username = username,
                Password = password
            });

            ui.ShowMessage(
                "User created: " + user.FullName + " (" + user.Username + ").");
        }
        catch (ServiceException ex)
        {
            ui.ShowError(ex.Message);
        }
        catch (DomainException ex)
        {
            ui.ShowDomainRule(ex.Message);
        }

        ui.Pause();
    }
}
