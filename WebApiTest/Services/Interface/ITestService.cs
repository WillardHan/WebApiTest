using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiTest.Services
{
    public interface ITestService
    {
        Task<List<string>> GetAll();
    }
}
