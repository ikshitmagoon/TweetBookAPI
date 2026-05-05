using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TweetBook.Domain;

namespace TweetBook.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext(options)
    {
        public DbSet<Post> Posts { get; set; }
    }
}

   
