using WebApi.Infrastructure.Repository;

namespace WebApiTest.Domain.Models
{
    public interface IComputerRepository : IRepository<Computer>
    {
        bool IsCodeExist(string code);
        Computer FingByCode(string code);
    }
}
