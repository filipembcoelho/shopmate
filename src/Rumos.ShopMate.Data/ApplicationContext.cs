using Microsoft.EntityFrameworkCore;
using Rumos.ShopMate.Data.Configurations;
using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Data;

public class ApplicationContext : DbContext

{
    public DbSet<User> Users { get; set; }
    public DbSet<ShoppingList> ShoppingLists { get; set; }

    // OnConfiguring method to configure the database connection
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.ApplyConfiguration(new ShoppingListConfiguration());

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }

    private void SeedData()
    {
        var seedData = MemoryDomainSeeder.CreateSeedData();

        // TODO: Implement the seeding logic to populate the database with the seed data.
        Users.AddRange(seedData.Users);
        //ShoppingLists = seedData.ShoppingLists;
        //Users = Users.AsReadOnly();
        //ShoppingLists = ShoppingLists.AsReadOnly();
    }

    public User RegisterUser(string fullName, string username, string password)
    {
        if (FindUserByUsername(username) != null)
        {
            throw new InvalidAccountException("Username already exists.");
        }

        var user = new User(fullName, username, password);
        Users.Add(user);

        return user;
    }

    public User Login(string username, string password)
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

    public User FindUserByUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }

        foreach (var user in Users)
        {
            if (user.Account.Username == username.Trim().ToLower())
            {
                return user;
            }
        }

        return null;
    }

    public ShoppingList CreateShoppingList(string name, User owner)
    {
        var shoppingList = new ShoppingList(name, owner);
        ShoppingLists.Add(shoppingList);

        return shoppingList;
    }

    public List<ShoppingList> GetShoppingListsFor(User user)
    {
        var result = new List<ShoppingList>();

        foreach (var shoppingList in ShoppingLists)
        {
            if (shoppingList.HasMember(user))
            {
                result.Add(shoppingList);
            }
        }

        return result;
    }
}
