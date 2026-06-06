using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace TweetBook.Authorization
{
    public class workForCOmpanyRequirement :IAuthorizationRequirement
    {
        public string domainName { get; }
        public workForCOmpanyRequirement(string DomainName)
        {
            domainName=DomainName;
        }
    }
}
