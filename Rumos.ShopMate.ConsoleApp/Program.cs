using System.Runtime.CompilerServices;
using Rumos.ShopMate.Data;
using Rumos.ShopMate.Domain.Model;
using Rumos.ShopMate.Domain.Model.Enums;


ShoppingListItem item = new ShoppingListItem("Milk", 1, Unit.Liter);

bool isValid = false;
do
{
    Console.Write("Enter name: ");
    string name = Console.ReadLine(); // L

    // if (name.Length > 1)
    // {
    //     item.ChangeName(name);
    //     isValid = true;
    // }
    // else
    // {
    //     Console.WriteLine("Name must be at least 2 characters long.");
    // }

    try
    {
        item.ChangeName(name);
        isValid = true;
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine();
        Console.WriteLine(ex.StackTrace);
        Console.WriteLine();
        Console.WriteLine("Name must be at least 2 characters long.");
    }
} while (!isValid);

ShoppingList list = new ShoppingList("Shopping List",
    new User(new Name("John", "Doe"), new Account("johndoe", "password123")));

User user = new User(new Name("Gonçalo", "Ferreira"), new Account("gferreira", "password123"));
ShoppingListMember member = new ShoppingListMember(user, ShoppingListRole.Viewer);

list.AddUser(member);

foreach (var listUser in list.Users)
{
    
}

// Database.SeedData();
//
// Start.Run();

// TPC: Password ASCII
// Shopping List
// Shopping List Item

// ligação User <=> ShoppingList
// Shared Shopping List (family and friends)
// Console app (evoluir)