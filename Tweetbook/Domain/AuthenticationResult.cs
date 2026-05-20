namespace TweetBook.Domain
{
    public class AuthenticationResult
    {
        public string token { get; set; }
        public string refresh_Token { get; set; }
        public bool success { get; set; }
        public IEnumerable<string> ErrorMessage { get; set; }
    }

}
