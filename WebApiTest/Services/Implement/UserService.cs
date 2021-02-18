using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Filters;
using WebApiTest.Attributes;
using WebApiTest.LifetimeInterfaces;
using WebApiTest.Models;

namespace WebApiTest.Services
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

        public async Task<List<string>> GetByName(string name)
        {
            var connectString = await connectionService.GetConnectionString();
            var list = await testService.GetAll();
            return list.Where(m => m == name).ToList();
        }
    }
}
