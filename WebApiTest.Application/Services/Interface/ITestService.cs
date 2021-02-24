using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiTest.Application.Services
{
    public interface ITestService
    {
        Task<List<string>> GetAll();
    }
}
