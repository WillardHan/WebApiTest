using WebApiTest.Infrastructure.Repository;

namespace WebApiTest.Domain.Models
{
    public interface ICompanyRepository : IRepository<Company>
    {
        bool IsCodeExist(string code);
        Company FingByCode(string code);
    }
}
