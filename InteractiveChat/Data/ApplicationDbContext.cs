using InteractiveChat.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InteractiveChat.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<FriendRequest>()
        .HasKey(fr => new { fr.SenderId, fr.ReceiverId });
            modelBuilder.Entity<Friendship>()
        .HasKey(fr => new { fr.UserId, fr.FriendId });
            //var user = new IdentityRole("user");
            //user.NormalizedName = "user";
            //modelBuilder.Entity<IdentityRole>().HasData(user);
            //modelBuilder.Entity<ApplicationUser>().ToTable("user");
        }
    }
}
