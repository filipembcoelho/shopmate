using Rumos.ShopMate.Domain.Model;
using Rumos.ShopMate.Domain.Model.Enums;

namespace Rumos.ShopMate.Data;

public static class MemoryDomainSeeder
{
    public static MemorySeedData CreateSeedData()
    {
        var seedData = new MemorySeedData();

        var ricardo = CreateUser(seedData, "Ricardo Jorge Bastos", "rbastos", "Ricardo1!");
        var filipe = CreateUser(seedData, "Filipe Brigas", "fbrigas", "Filipe2!");
        var bruno = CreateUser(seedData, "Bruno Alexandre", "balexandre", "Bruno33!");
        var ana = CreateUser(seedData, "Ana Martins", "amartins", "AnaShop4!");
        var joana = CreateUser(seedData, "Joana Santos", "jsantos", "Joana55!");

        var groceries = CreateShoppingList(seedData, "Weekly groceries", ricardo);
        groceries.AddItem("Milk", 2, Unit.Liter, ricardo);
        groceries.AddItem("Bread", 1, Unit.Each, ricardo);
        groceries.AddItem("Apples", 6, Unit.Each, new Category("Fruit"), ricardo);
        groceries.ShareWith(filipe, ShoppingListRole.Editor, ricardo);
        groceries.ShareWith(bruno, ShoppingListRole.Viewer, ricardo);

        var party = CreateShoppingList(seedData, "Birthday party", filipe);
        party.AddItem("Juice", 3, Unit.Liter, filipe);
        party.AddItem("Cake", 1, Unit.Each, filipe);
        party.ShareWith(ricardo, ShoppingListRole.Editor, filipe);
        party.ShareWith(ana, ShoppingListRole.Editor, filipe);

        var office = CreateShoppingList(seedData, "Office snacks", ana);
        office.AddItem("Coffee", 2, Unit.Package, ana);
        office.AddItem("Cookies", 4, Unit.Package, ana);
        office.ShareWith(joana, ShoppingListRole.Viewer, ana);

        return seedData;
    }

    private static User CreateUser(MemorySeedData seedData, string fullName, string username, string password)
    {
        var user = new User(fullName, username, password);
        seedData.Users.Add(user);

        return user;
    }

    private static ShoppingList CreateShoppingList(MemorySeedData seedData, string name, User owner)
    {
        var shoppingList = new ShoppingList(name, owner);
        seedData.ShoppingLists.Add(shoppingList);

        return shoppingList;
    }
}
