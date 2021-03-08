using WebApi.Infrastructure.Repository;

namespace WebApiTest2.Domain.Models
{
    public interface IUserRepository : IRepository<User>
    {
        bool IsCodeExist(string code);
        User FingByCode(string code);
    }
}
