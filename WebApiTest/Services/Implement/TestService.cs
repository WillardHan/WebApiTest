using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Attributes;
using WebApiTest.LifetimeInterfaces;

namespace WebApiTest.Services
{
    [Service(ServiceLifetime.Scoped)]
    public class TestService : ITestService, IScopedInterface
    {
        public async Task<List<string>> GetAll()
        {
            return await Task.FromResult(new List<string> {
                "11",
                "22",
                "33"
            });
        }
    }
}
