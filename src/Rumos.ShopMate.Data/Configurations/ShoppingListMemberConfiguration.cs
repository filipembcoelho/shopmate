using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Data.Configurations;

internal sealed class ShoppingListMemberConfiguration : IEntityTypeConfiguration<ShoppingListMember>
{
    public void Configure(EntityTypeBuilder<ShoppingListMember> builder)
    {
        // Properties
        builder.ToTable("ShoppingListMembers");
        builder.HasKey(member => member.Id);

        builder.Property(member => member.Role)
            .IsRequired();

        // Relationships
        builder.HasOne(member => member.User)
            .WithMany()
            .HasForeignKey(member => member.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(member => new { member.ShoppingListId, member.UserId })
            .IsUnique();
    }
}
