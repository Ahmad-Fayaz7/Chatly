using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractiveChat.Models
{
    public class MessageConfiguration : IEntityTypeConfiguration<InteractiveChat.Models.Message>
    {
        public void Configure(EntityTypeBuilder<InteractiveChat.Models.Message> builder)
        {
            // Configure Messages table
            
            // Table name
            builder.ToTable("Messages");

            builder.Property(m => m.ConversationId)
                .IsRequired();
            
            builder.Property(m => m.Content)
                .IsRequired();
            
            builder.Property(m => m.Timestamp)
                .IsRequired();
            
            // Primary key
            builder.HasKey(m => m.MessageId);
            
            // Relationships
            builder.HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .HasConstraintName("FK_Messages_ConversationId")
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(m => m.Sender)
                .WithMany(s => s.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .HasConstraintName("FK_Messages_SenderId")
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(m => m.Recipient)
                .WithMany(r => r.ReceivedMessages)
                .HasForeignKey(m => m.RecipientId)
                .HasConstraintName("FK_Messages_RecipientId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}