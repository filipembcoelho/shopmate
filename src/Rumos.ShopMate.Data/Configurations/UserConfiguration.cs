using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Data.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    private static readonly ValueComparer<List<string>> StringListComparer = new(
        (left, right) => left.SequenceEqual(right),
        value => value.Aggregate(0, (hash, item) => HashCode.Combine(hash, item.GetHashCode())),
        value => value.ToList());

    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Properties
        builder.ToTable("Users");
        builder.HasKey(user => user.Id);

        // Relationships
        builder.OwnsOne(user => user.Name, name =>
        {
            // Name value object properties
            name.Property(value => value.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(40)
                .IsRequired();

            name.Property(value => value.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(40)
                .IsRequired();

            name.Property(value => value.MiddleNames)
                .HasColumnName("MiddleNames")
                .HasConversion(
                    values => JsonSerializer.Serialize(values),
                    value => JsonSerializer.Deserialize<List<string>>(value) ?? new List<string>(),
                    StringListComparer)
                .IsRequired();
        });

        builder.Navigation(user => user.Name)
            .IsRequired();

        // User and account relationship
        builder.HasOne(user => user.Account)
            .WithOne(account => account.User)
            .HasForeignKey<Account>(account => account.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
