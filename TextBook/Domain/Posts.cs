using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TweetBook.Domain
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string userId { get; set; }
        [ForeignKey(nameof(userId))]
        public IdentityUser User { get; set; }
    }
}
