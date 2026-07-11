using Rumos.ShopMate.ConsoleApp.Menus;
using Rumos.ShopMate.ConsoleApp.Ui;

namespace Rumos.ShopMate.ConsoleApp.Application;

public class ShopMateConsoleApplication
{
    public void Run()
    {
        var ui = new ConsoleUi();
        var mainMenu = new MainMenu(ui);

        mainMenu.Show();
    }
}
