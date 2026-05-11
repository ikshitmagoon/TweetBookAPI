using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Principal;
using TweetBook.Contract.V1;
using TweetBook.Contract.V1.Requests;
using TweetBook.Contract.V1.Responses;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoute.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password);

            if (!authResponse.success)
            {
                return BadRequest(new AuthFailResponse
                {
                    ErrorMessage = authResponse.ErrorMessage
                });
            }
            return Ok(new AuthSuccessResponse
            {
                token= authResponse.token
            });
        }
    }
}
