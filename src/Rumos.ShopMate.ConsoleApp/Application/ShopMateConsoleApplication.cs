using Rumos.ShopMate.ConsoleApp.Menus;

namespace Rumos.ShopMate.ConsoleApp.Application;

public class ShopMateConsoleApplication(MainMenu mainMenu)
{
    public void Run()
    {
        mainMenu.Show();
    }
}
