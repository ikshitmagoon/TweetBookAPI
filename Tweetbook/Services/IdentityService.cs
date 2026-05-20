using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.AccessControl;
using System.Security.Claims;
using TweetBook.Data;
using TweetBook.Domain;
using TweetBook.Options;

namespace TweetBook.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JWTSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DataContext _context;

        public IdentityService(UserManager<IdentityUser> userManager,JWTSettings jWTSettings,TokenValidationParameters tokenValidationParameters, DataContext dataContext)
        {
            _userManager = userManager;
            _jwtSettings = jWTSettings;
            _tokenValidationParameters=tokenValidationParameters;
            _context = dataContext;
        }
        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string Refresh_Token)
        {
            var validatedToken= GetPrincipalFromToken(token);
            if(validatedToken==null)
            {
                return new AuthenticationResult
                {
                    ErrorMessage = new[] { "Invalid Token" }
                };
            }
            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc).AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResult { ErrorMessage = new[] { "This token hasn't expired yet" } };
            }

                var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storeRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x=> x.Token == Refresh_Token);


            if (storeRefreshToken == null)
            {
                return new AuthenticationResult { ErrorMessage = new[] { "This refresh token does not exist" } };
            }
            if (DateTime.UtcNow > storeRefreshToken.ExpiryDate)
            {

                return new AuthenticationResult { ErrorMessage = new[] { "This refresh token has expired" } };
            }
            if(storeRefreshToken.isValidated)
            {
                return new AuthenticationResult { ErrorMessage = new[] { "This refresh token has been Invalidated" } };
            }
            if (storeRefreshToken.used)
            {
                return new AuthenticationResult { ErrorMessage = new[] { "This refresh token has been used " } };
            }
            if(storeRefreshToken.JwtId!= jti)
            {
                return new AuthenticationResult { ErrorMessage = new[] { "This refresh token does not match this JWT" } };
            }
            storeRefreshToken.used = true;
            _context.RefreshTokens.Update(storeRefreshToken);
            await _context.SaveChangesAsync();
            var user=await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "Id").Value);
            return await GenerateJwtToken(user);

        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var prinicipal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if(!isJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return prinicipal;
            }
            catch
            {
                return null;
            }
        }
        private bool isJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
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
            
            return await GenerateJwtToken(newUser);
        }
         private async Task<AuthenticationResult> GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_jwtSettings.secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("Id",user.Id)

                }),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refeshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(), 
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };
            await _context.RefreshTokens.AddAsync(refeshToken);
            await _context.SaveChangesAsync();
            return new AuthenticationResult
            {
                success = true,
                token = tokenHandler.WriteToken(token),
                refresh_Token = refeshToken.Token
            };
           
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User == null)
            {
                return new AuthenticationResult
                {

                    ErrorMessage = new[] { "User Does Not Exist" }
                };

            }
          
            var CheckPassword = await _userManager.CheckPasswordAsync(User,password);

            if (!CheckPassword)
            {
                return new AuthenticationResult
                {

                    ErrorMessage = new[] { "Password Combimation is Wrong" }
                };
            }
           return await GenerateJwtToken(User);
        }

    }
}
