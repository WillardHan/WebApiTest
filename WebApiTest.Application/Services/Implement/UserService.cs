using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using WebApiTest.Application.Models;
using WebApi.Infrastructure.Attributes;
using WebApi.Infrastructure.LifetimeInterfaces;

namespace WebApiTest.Application.Services
{
    [Service(ServiceLifetime.Scoped)]
    public class UserService : IUserService, IScopedInterface
    {
        //private readonly Lazy<IConnectionService> connectionService;
        private readonly IConnectionService connectionService;
        private readonly ITestService testService;
        private readonly IOptions<TestSetting> testSetting;
        public UserService(
            IConnectionService connectionService, 
            ITestService testService, 
            IOptions<TestSetting> testSetting
            )
        {
            this.connectionService = connectionService;
            this.testService = testService;
            this.testSetting = testSetting;
        }

        public async Task<List<string>> GetByName(string name, CancellationToken cancellationToken)
        {
            var connectString = await connectionService.GetConnectionString();
            var list = await testService.GetAll();
            return list.Where(m => m == name).ToList();
        }
    }
}
