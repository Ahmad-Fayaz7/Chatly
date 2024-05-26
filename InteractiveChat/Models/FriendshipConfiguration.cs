using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractiveChat.Models;

public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {
        // Configure the Friendship entity

        // Set table name
        builder.ToTable("Friendships");

        // Configure primary key
        builder.HasKey(f => new { f.UserId, f.FriendId });

        builder.Property(f => f.UserId)
            .HasColumnOrder(0);

        builder.Property(f => f.FriendId)
            .HasColumnOrder(1);

        // Configure UserId foreign key
        builder.HasOne(f => f.User)
            .WithMany(u => u.Friendships)
            .HasForeignKey(f => f.UserId)
            .IsRequired();

        // Configure FriendId foreign key
        builder.HasOne(f => f.Friend)
            .WithMany()
            .HasForeignKey(f => f.FriendId)
            .IsRequired();

        // Configure friendship date
        builder.Property(f => f.FriendshipDate)
            .IsRequired();
    }
}