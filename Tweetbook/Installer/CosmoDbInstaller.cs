//using Cosmonaut;
//using Cosmonaut.Extensions.Microsoft.DependencyInjection;
//using Microsoft.Azure.Documents.Client;
using Microsoft.Data.SqlClient;
using TweetBook.Domain;

namespace TweetBook.Installer
{
    public class CosmoDbInstaller : Iinstaller
    {
        void Iinstaller.InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //var cosmoStoreSettings=new CosmosStoreSettings(
            //    configuration["CosmoSettings:DatabaseName"],
            //     configuration["CosmoSettings:Account"],
            //     configuration["CosmoSettings:Key"],
            //     new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp }
            //    );

            //services.AddCosmosStore<CosmosPostDTO>(cosmoStoreSettings);
        }
    }
}
