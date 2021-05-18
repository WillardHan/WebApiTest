using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using WebApi.Infrastructure.Attributes;
using WebApi.Infrastructure.LifetimeInterfaces;
using WebApiTest.Domain.Mappings;
using WebApiTest.Domain.Models;

namespace WebApiTest.Domain
{
    public class MongoDatabaseContext
    {
        private readonly IMongoDatabase database;

        public MongoDatabaseContext(IConfiguration configuration)
        {
            OperationHistoryMapping.Configure();
            var client = new MongoClient(configuration.GetValue<string>("MONGO_CONNECTIONSTRING"));
            database = client.GetDatabase("WebApiTestDb");
        }

        public IMongoCollection<OperationHistory> OperationHistory
        {
            get
            {
                return database.GetCollection<OperationHistory>("OperationHistory");
            }
        }
    }
}
