using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Data.Configurations;

internal sealed class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        // Properties
        builder.ToTable("Activities");
        builder.HasKey(activity => activity.Id);

        builder.Property(activity => activity.Description)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(activity => activity.CreatedAt)
            .IsRequired();

        AuditableConfiguration.Configure(builder);

        // The ShoppingList relationship is configured in ShoppingListConfiguration.
    }
}
