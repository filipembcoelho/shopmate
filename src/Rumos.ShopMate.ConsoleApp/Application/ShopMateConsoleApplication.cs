using Rumos.ShopMate.ConsoleApp.Menus;
using Rumos.ShopMate.ConsoleApp.Ui;
using Rumos.ShopMate.Data;

namespace Rumos.ShopMate.ConsoleApp.Application;

public class ShopMateConsoleApplication(ApplicationContext context)
{
    public void Run()
    {
        var ui = new ConsoleUi();
        var mainMenu = new MainMenu(ui, context);

        mainMenu.Show();
    }
}
