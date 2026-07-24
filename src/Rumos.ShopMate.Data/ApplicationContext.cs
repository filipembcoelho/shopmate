using Microsoft.EntityFrameworkCore;
using Rumos.ShopMate.Data.Configurations;
using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Data;

public class ApplicationContext : DbContext
{
    private const string DefaultConnectionString = "";

    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<ShoppingList> ShoppingLists { get; set; }
    public DbSet<ShoppingListItem> ShoppingListItems { get; set; }
    public DbSet<ShoppingListMember> ShoppingListMembers { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryRule> CategoryRules { get; set; }

    public ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(DefaultConnectionString);
        }
    }

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
