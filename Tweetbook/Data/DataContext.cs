using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TweetBook.Domain;

namespace TweetBook.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext(options)
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RefreshToken>()
                .Property(r => r.Token)
                .ValueGeneratedNever();

            modelBuilder.Entity<Tags>()
     .HasOne(t => t.Post)
     .WithMany(p => p.Tags)
     .HasForeignKey(t => t.PostId)
     .OnDelete(DeleteBehavior.Cascade);
        }

    }
}

   
