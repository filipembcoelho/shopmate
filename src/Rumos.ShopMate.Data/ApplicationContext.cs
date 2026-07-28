using Microsoft.EntityFrameworkCore;
using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<ShoppingList> ShoppingLists { get; set; }
    public DbSet<ShoppingListItem> ShoppingListItems { get; set; }
    public DbSet<ShoppingListMember> ShoppingListMembers { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryRule> CategoryRules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }

    public void SeedData()
    {
        var seedData = MemoryDomainSeeder.CreateSeedData();

        Users.AddRange(seedData.Users);
        ShoppingLists.AddRange(seedData.ShoppingLists);
    }
}
