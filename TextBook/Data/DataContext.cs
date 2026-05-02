using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TweetBook.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext(options)
    {
    }
}
