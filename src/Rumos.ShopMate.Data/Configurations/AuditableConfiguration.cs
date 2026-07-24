using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rumos.ShopMate.Domain.Model.Common;

namespace Rumos.ShopMate.Data.Configurations;

internal static class AuditableConfiguration
{
    public static void Configure<TEntity>(EntityTypeBuilder<TEntity> builder)
        where TEntity : AuditableEntity
    {
        // Audit properties
        builder.Property(entity => entity.CreatedBy)
            .HasMaxLength(100);

        builder.Property(entity => entity.UpdatedBy)
            .HasMaxLength(100);

        builder.Property(entity => entity.Created)
            .IsRequired();

        builder.Property(entity => entity.Updated)
            .IsRequired();
    }
}
