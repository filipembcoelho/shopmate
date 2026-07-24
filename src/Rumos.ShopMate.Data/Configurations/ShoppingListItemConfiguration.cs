using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Data.Configurations;

internal sealed class ShoppingListItemConfiguration : IEntityTypeConfiguration<ShoppingListItem>
{
    public void Configure(EntityTypeBuilder<ShoppingListItem> builder)
    {
        // Properties
        builder.ToTable("ShoppingListItems");
        builder.HasKey(item => item.Id);

        builder.Property(item => item.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(item => item.Quantity)
            .IsRequired();

        builder.Property(item => item.Unit)
            .IsRequired();

        builder.Property(item => item.IsCompleted)
            .IsRequired();

        AuditableConfiguration.Configure(builder);

        // Relationships
        builder.HasOne(item => item.Category)
            .WithMany()
            .HasForeignKey(item => item.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
