using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Data;

public static class Database
{
    private static List<User> _users;
    private static List<ShoppingList> _shoppingLists;

    public static IReadOnlyList<User> Users { get; set; }
    public static IReadOnlyList<ShoppingList> ShoppingLists { get; set; }

    static Database()
    {
        _users = new List<User>();
        _shoppingLists = new List<ShoppingList>();
        Users = _users.AsReadOnly();
        ShoppingLists = _shoppingLists.AsReadOnly();
    }

    public static void SeedData()
    {
        var seedData = MemoryDomainSeeder.CreateSeedData();

        _users = seedData.Users;
        _shoppingLists = seedData.ShoppingLists;
        Users = _users.AsReadOnly();
        ShoppingLists = _shoppingLists.AsReadOnly();
    }

    public static User RegisterUser(string fullName, string username, string password)
    {
        if (FindUserByUsername(username) != null)
        {
            throw new InvalidAccountException("Username already exists.");
        }

        var user = new User(fullName, username, password);
        _users.Add(user);

        return user;
    }

    public static User Login(string username, string password)
    {
        var user = FindUserByUsername(username);

        if (user == null)
        {
            return null;
        }

        if (user.Account.Password != password)
        {
            return null;
        }

        return user;
    }

    public static User FindUserByUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }

        foreach (var user in _users)
        {
            if (user.Account.Username == username.Trim().ToLower())
            {
                return user;
            }
        }

        return null;
    }

    public static ShoppingList CreateShoppingList(string name, User owner)
    {
        var shoppingList = new ShoppingList(name, owner);
        _shoppingLists.Add(shoppingList);

        return shoppingList;
    }

    public static List<ShoppingList> GetShoppingListsFor(User user)
    {
        var result = new List<ShoppingList>();

        foreach (var shoppingList in _shoppingLists)
        {
            if (shoppingList.HasMember(user))
            {
                result.Add(shoppingList);
            }
        }

        return result;
    }
}
