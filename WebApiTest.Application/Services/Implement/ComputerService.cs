using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Domain;
using WebApi.Infrastructure.Attributes;
using WebApiTest.Application.Models;
using WebApi.Infrastructure.Utility;
using WebApi.Infrastructure.LifetimeInterfaces;

namespace WebApiTest.Application.Services
{
    [Service(ServiceLifetime.Scoped)]
    public class ComputerService : IComputerService, IScopedInterface
    {
        private readonly IConnectionService connectionService;
        private readonly DatabaseContext databaseContext;
        public ComputerService(
            IConnectionService connectionService,
            DatabaseContext databaseContext
            )
        {
            this.connectionService = connectionService;
            this.databaseContext = databaseContext;
        }

        public async Task<IEnumerable<ComputerModel>> GetAll()
        {
            var models = databaseContext.Computer.ToList();
            return models.ToDTO<IEnumerable<ComputerModel>>();
        }
    }
}
