namespace TweetBook.Installer
{
    public class mvcInstaller:Iinstaller
    {
       public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo { Title = "TweetBook API", Version = "v1" });
            });
        }
    }
}
