using WebApiTest.Domain.Models;
using System.Threading.Tasks;
using WebApi.Infrastructure.LifetimeInterfaces;
using WebApi.Infrastructure.Attributes;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Collections.Generic;

namespace WebApiTest.Domain.Repositories
{
    [Service(ServiceLifetime.Scoped)]
    public class OperationHistoryRepository : IOperationHistoryRepository, IScopedInterface
    {
        private readonly MongoDatabaseContext context;

        public OperationHistoryRepository(MongoDatabaseContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(OperationHistory model)
        {
            await context.OperationHistory.InsertOneAsync(model);
        }

        public async Task<List<OperationHistory>> FindAsync()
        {
            var filter = Builders<OperationHistory>.Filter.Empty;
            var result = await context.OperationHistory.FindAsync(filter);
            return await result.ToListAsync();
        }
    }
}
