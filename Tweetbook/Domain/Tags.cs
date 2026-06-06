using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TweetBook.Domain
{
    public class Tags
    {
        [Key]
        public Guid CreaterId { get; set; }   // Primary key

        public Guid PostId { get; set; } // Foreign key to Post
        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }

        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
