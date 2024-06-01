using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractiveChat.Models;

public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {
        // Configure the FriendRequest entity

        // Table name
        builder.ToTable("Friendships");

        // Primary Key
        builder.HasKey(fr => new { fr.UserId, fr.FriendId });

        // Relationships
        builder.HasOne(fr => fr.User)
            .WithMany(u => u.Friendships)
            .HasForeignKey(fr => fr.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(fr => fr.Friend)
            .WithMany(u => u.FriendsOf)
            .HasForeignKey(fr => fr.FriendId)
            .OnDelete(DeleteBehavior.Restrict);

        // Additional properties configuration (if any)
        builder.Property(fr => fr.FriendshipDate)
            .IsRequired();
    }
}