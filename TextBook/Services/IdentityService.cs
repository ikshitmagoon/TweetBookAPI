using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TweetBook.Domain;
using TweetBook.Options;

namespace TweetBook.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JWTSettings _jwtSettings;

        public IdentityService(UserManager<IdentityUser> userManager,JWTSettings jWTSettings)
        {
            _userManager = userManager;
            _jwtSettings = jWTSettings;
        }
        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existingUser =await  _userManager.FindByEmailAsync(email);
            if(existingUser != null)
            {
                return new AuthenticationResult
                {

                    ErrorMessage = new[] { "User with this email address already exists" }
                };
               
            }
            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };
            var createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    ErrorMessage = createdUser.Errors.Select(x => x.Description)
                }; 
            }
            var tokenHandler=new JwtSecurityTokenHandler();
            var key=System.Text.Encoding.ASCII.GetBytes(_jwtSettings.secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Email,newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("Id",newUser.Id)

                }),
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

            var token=tokenHandler.CreateToken(tokenDescriptor);
            return new AuthenticationResult
            {
                success = true,
                token = tokenHandler.WriteToken(token)
            };

        }
    }
}
