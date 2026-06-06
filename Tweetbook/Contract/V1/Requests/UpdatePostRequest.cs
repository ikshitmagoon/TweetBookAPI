using TweetBook.Domain;

namespace TweetBook.Contract.V1.Requests
{
    public class UpdatePostRequest
    {
        public string Name { get; set; }
        public Tags tag { get; set; }
    }
}
