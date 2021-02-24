using System.Threading.Tasks;

namespace WebApiTest.Application.Services
{
    public interface IConnectionService
    {
        Task<string> GetConnectionString();
    }
}
