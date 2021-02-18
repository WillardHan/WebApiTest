using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WebApiTest.Attributes;
using WebApiTest.LifetimeInterfaces;

namespace WebApiTest.Services
{
    [Service(ServiceLifetime.Singleton)]
    public class ConnectionService : IConnectionService, ISingletonInterface
    {
        public IConfiguration configuration { get; set; }
        public ConnectionService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<string> GetConnectionString()
        {
            return await Task.FromResult(configuration.GetValue<string>("ConnectionString"));
        }
    }
}
