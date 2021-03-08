using System.Threading.Tasks;

namespace WebApiTest2.Application.Services
{
    public interface IConnectionService
    {
        Task<string> GetConnectionString();
    }
}
