using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rumos.Acd.DataDemo;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        //var cs = "Server=localhost;Database=EfDemo;Trusted_Connection=True;TrustServerCertificate=True;";
        var cs = "Server=94.46.180.24;Database=codedev2026;User Id=fcoelho; Password=Mrg?1r249;TrustServerCertificate=True;";
        options.UseSqlServer(cs);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}

class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.IdCard).HasMaxLength(20);
        builder.Property(u => u.TaxNumber).HasMaxLength(20);
    }
}
