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
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailResponse
                {
                    ErrorMessage = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                }
                    );
            }
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
        [HttpPost(ApiRoute.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        { 
            var authResponse = await _identityService.LoginAsync(request.Email, request.password);

            if (!authResponse.success)
            {
                return BadRequest(new AuthFailResponse
                {
                    ErrorMessage = authResponse.ErrorMessage
                });
            }
            return Ok(new AuthSuccessResponse
            {
                token = authResponse.token,
                Refresh_token=authResponse.refresh_Token

            });
        }
        [HttpPost(ApiRoute.Identity.Refresh)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var authResponse = await _identityService.RefreshTokenAsync(request.Token, request.Refresh_Token);

            if (!authResponse.success)
            {
                return BadRequest(new AuthFailResponse
                {
                    ErrorMessage = authResponse.ErrorMessage
                });
            }
            return Ok(new AuthSuccessResponse
            {
                token = authResponse.token,
                 Refresh_token = authResponse.refresh_Token
            });
        }
    }
}
