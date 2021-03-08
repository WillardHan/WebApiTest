using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest2.Domain;
using WebApi.Infrastructure.Attributes;
using WebApiTest2.Application.Models;
using WebApi.Infrastructure.Utility;
using WebApi.Infrastructure.LifetimeInterfaces;
using WebApiTest.Rpc.Services;

namespace WebApiTest2.Application.Services
{
    [Service(ServiceLifetime.Scoped)]
    public class UserService : IUserService, IScopedInterface
    {
        private readonly IConnectionService connectionService;
        private readonly ICompanyService companyService;
        private readonly DatabaseContext databaseContext;
        public UserService(
            IConnectionService connectionService,
            ICompanyService companyService,
            DatabaseContext databaseContext
            )
        {
            this.connectionService = connectionService;
            this.companyService = companyService;
            this.databaseContext = databaseContext;
        }

        public async Task<IEnumerable<UserModel>> GetAll()
        {
            var test = await companyService.GetAllAsync();
            var models = databaseContext.User.ToList();
            return test.ToDTO<IEnumerable<UserModel>>();
        }
    }
}
