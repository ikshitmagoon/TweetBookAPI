using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TweetBook.Domain;

namespace TweetBook.Contract.V1.Responses
{
    public class CreateResponse
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string userId { get; set; }
      
        public IEnumerable<TagResponse> Tags { get; set; } 
    }
}
