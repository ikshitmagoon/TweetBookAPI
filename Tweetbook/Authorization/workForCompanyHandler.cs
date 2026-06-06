using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TweetBook.Authorization
{
    public class workForCompanyHandler : AuthorizationHandler<workForCOmpanyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, workForCOmpanyRequirement requirement)
        {
            var emailAdress = context.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            if (emailAdress.EndsWith(requirement.domainName)){
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            else
            {
                context.Fail();
                return Task.CompletedTask;
            }
        }
    }
}
