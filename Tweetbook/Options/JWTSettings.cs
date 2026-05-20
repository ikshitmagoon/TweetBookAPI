namespace TweetBook.Options
{
    public class JWTSettings
    {
        public string secret { get; set; }
        public TimeSpan TokenLifetime { get; set; }
    }
}
