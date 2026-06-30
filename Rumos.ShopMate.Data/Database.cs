using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Data;

public static class Database
{
    public static List<User> Users;

    public static void SeedData()
    {
        Users = new List<User>();
        
        string pass = "12345678";
        
        Users.Add(new User("Ricardo Jorge Bastos", "rbastos", pass));
        Users.Add(new User("Filipe Brigas", "fbrigas", pass));
        Users.Add(new User(new Name("Bruno", "Alexandre"), new Account("balexandre", pass)));
    }
}