using System.Runtime.CompilerServices;

namespace TweetBook.Entensions
{
    public static class generalExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }
            return httpContext.User.Claims.Single(x => x.Type == "Id").Value;
           
        }
    }
}
