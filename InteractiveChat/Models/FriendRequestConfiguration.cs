using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractiveChat.Models;

public class FriendRequestConfiguration : IEntityTypeConfiguration<FriendRequest>
{
    public void Configure(EntityTypeBuilder<FriendRequest> builder)
    {
        // Configure the FriendRequest entity

        // Table name
        builder.ToTable("FriendRequests");

        // Primary Key
        builder.HasKey(fr => new { fr.SenderId, fr.ReceiverId });

        // Relationships
        builder.HasOne(fr => fr.SenderUser)
            .WithMany(u => u.SentFriendRequests)
            .HasForeignKey(fr => fr.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(fr => fr.ReceiverUser)
            .WithMany(u => u.ReceivedFriendRequests)
            .HasForeignKey(fr => fr.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        // Additional properties configuration (if any)
        builder.Property(fr => fr.InvitationDate)
            .IsRequired();
    }
}