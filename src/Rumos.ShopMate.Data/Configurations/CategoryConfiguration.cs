using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Data.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Properties
        builder.ToTable("Categories");
        builder.HasKey(category => category.Id);

        builder.Property(category => category.Value)
            .HasMaxLength(30)
            .IsRequired();

        AuditableConfiguration.Configure(builder);

        // Relationships are configured in ShoppingListItemConfiguration.
    }
}
