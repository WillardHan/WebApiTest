using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Infrastructure.Attributes;
using WebApi.Infrastructure.LifetimeInterfaces;

namespace WebApiTest.Application.Services
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
