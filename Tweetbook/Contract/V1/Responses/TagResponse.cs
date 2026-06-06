using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TweetBook.Domain;

namespace TweetBook.Contract.V1.Responses
{
    public class TagResponse
    {
        public Guid CreaterId { get; set; }   // Primary key

        public Guid PostId { get; set; } // Foreign key to Post
    
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
