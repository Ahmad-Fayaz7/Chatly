using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractiveChat.Models
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            // Configure FirstName property
            builder.Property(u => u.FirstName)
                   .IsRequired();

            // Configure LastName property
            builder.Property(u => u.LastName)
                   .IsRequired();

            // Configure ProfilePicUrl property
            builder.Property(u => u.ProfilePicUrl)
                   .IsRequired(false);

            // Configure CreationTime property
            builder.Property(u => u.CreationTime)
                   .IsRequired();

            //// Configure SentFriendRequests navigation property
            //builder.HasMany(u => u.SentFriendRequests)
            //       .WithOne(fr => fr.SenderUser)
            //       .HasForeignKey(fr => fr.SenderId)
            //       .IsRequired();

            //// Configure ReceivedFriendRequests navigation property
            //builder.HasMany(u => u.ReceivedFriendRequests)
            //       .WithOne(fr => fr.ReceiverUser)
            //       .HasForeignKey(fr => fr.ReceiverId)
            //       .IsRequired();

            //// Configure Friends navigation property
            //builder.HasMany(u => u.Friends)
            //       .WithOne(f => f.User)
            //       .HasForeignKey(f => f.UserId)
            //       .IsRequired();
        }
    }
}
