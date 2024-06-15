using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractiveChat.Models;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        // Configure Conversations table
        
        // Table name
        builder.ToTable("Conversations");
        
        // Primary key
        builder.HasKey(c => c.ConversationId);
        
        // Relationships 
        builder.HasMany(c => c.Participants)
            .WithMany(u => u.Conversations)
            .UsingEntity(j => j.ToTable("ConversationParticipants"));
    }
}