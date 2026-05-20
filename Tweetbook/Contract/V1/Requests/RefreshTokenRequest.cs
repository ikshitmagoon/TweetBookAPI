namespace TweetBook.Contract.V1.Requests
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string Refresh_Token { get; set; }
    }
}
