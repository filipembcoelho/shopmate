using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Data.Configurations;

internal sealed class CategoryRuleConfiguration : IEntityTypeConfiguration<CategoryRule>
{
    private static readonly ValueComparer<List<string>> StringListComparer = new(
        (left, right) => left.SequenceEqual(right),
        value => value.Aggregate(0, (hash, item) => HashCode.Combine(hash, item.GetHashCode())),
        value => value.ToList());

    public void Configure(EntityTypeBuilder<CategoryRule> builder)
    {
        // Properties
        builder.ToTable("CategoryRules");
        builder.HasKey(rule => rule.Id);

        builder.Property(rule => rule.CategoryName)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(rule => rule.Words)
            .HasConversion(
                values => JsonSerializer.Serialize(values),
                value => JsonSerializer.Deserialize<List<string>>(value) ?? new List<string>(),
                StringListComparer)
            .IsRequired();

        AuditableConfiguration.Configure(builder);

        // This entity currently has no relationships.
    }
}
