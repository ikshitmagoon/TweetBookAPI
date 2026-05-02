namespace TweetBook.Installer
{
    public static class installerExtension
    {
         public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = typeof(Program).Assembly.ExportedTypes.Where(x => typeof(Iinstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance).Cast<Iinstaller>().ToList();
            installers.ForEach(installers => installers.InstallServices(services, configuration));

        }
    }
}
