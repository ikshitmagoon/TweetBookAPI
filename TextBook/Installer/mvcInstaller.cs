using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using TweetBook.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Principal;
using TweetBook.Services;

namespace TweetBook.Installer
{
    public class mvcInstaller:Iinstaller
    {
       public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.AddScoped<IIdentityService,IdentityService>(); 
            var jwtSettings=new JWTSettings();
            configuration.Bind(nameof(JWTSettings), jwtSettings);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.secret)),
                    ValidateIssuer=false,
                    ValidateAudience=false,
                    RequireExpirationTime=false,
                    ValidateLifetime=true
                };

            });
            services.AddSingleton(jwtSettings);

            // ✅ Add this at the top of Startup.cs or Program.cs
           
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TweetBook API",
                    Version = "v1"
                });

               

                // 🔑 Define the Bearer security scheme
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat= "JWT"
                });

                // 🔑 Add the requirement so Swagger knows to use it
                x.AddSecurityRequirement(securtiy => new OpenApiSecurityRequirement
                {

                    [new OpenApiSecuritySchemeReference("Bearer", securtiy)] = new List<string>()

                });
            });

        }
    }
}
