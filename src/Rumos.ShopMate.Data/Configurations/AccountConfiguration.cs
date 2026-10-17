using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rumos.ShopMate.Domain.Constants;
using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Data.Configurations;

internal sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        // Properties
        builder.ToTable("Accounts");
        builder.HasKey(account => account.Id);

        builder.Property(account => account.Username)
            .HasMaxLength(40)
            .IsRequired();

        builder.HasIndex(account => account.Username)
            .IsUnique();

        builder.Property(account => account.PasswordHash)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(account => account.Role)
            .HasMaxLength(20)
            .HasDefaultValue(AppRoles.User)
            .IsRequired();

        AuditableConfiguration.Configure(builder);

        // Relationships are configured in UserConfiguration.
    }

    // private void CreateProperties(EntityTypeBuilder<Account> builder) {}
    // private void CreateRelationships(EntityTypeBuilder<Account> builder) {}
}
