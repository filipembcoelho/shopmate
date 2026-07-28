using Microsoft.EntityFrameworkCore;
using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Data;

public class ApplicationContext : DbContext
{
    private const string ConnectionStringVariableName = "Server=94.46.180.24;Database=ShopMate;User Id=shopmate;Password=O3!ybtOOcr0drg2&;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";

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
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        
        if (string.IsNullOrWhiteSpace(ConnectionStringVariableName))
        {
            throw new InvalidOperationException(
                "The ShopMate connection string was not configured.");
        }

        optionsBuilder.UseSqlServer(ConnectionStringVariableName);
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
