namespace TweetBook.Installer
{
    public interface Iinstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
