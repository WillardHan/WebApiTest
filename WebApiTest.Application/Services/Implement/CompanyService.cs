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
    public class CompanyService : ICompanyService, IScopedInterface
    {
        private readonly IConnectionService connectionService;
        private readonly DatabaseContext databaseContext;
        public CompanyService(
            IConnectionService connectionService,
            DatabaseContext databaseContext
            )
        {
            this.connectionService = connectionService;
            this.databaseContext = databaseContext;
        }

        public async Task<IEnumerable<CompanyModel>> GetAll()
        {
            //var connectionString = await connectionService.GetConnectionString();
            //using (var connection = new SqlConnection())
            //{
            //    connection.ConnectionString = connectionString;
            //    connection.Open();
            //    var sql = $@"SELECT * FROM Company";
            //    return await connection.QueryAsync<CompanyModel>(sql); 
            //}

            var models = databaseContext.Company.ToList();
            return models.ToDTO<IEnumerable<CompanyModel>>();
        }
    }
}
