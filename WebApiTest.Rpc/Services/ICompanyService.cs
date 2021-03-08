using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Rpc.DTOs;

namespace WebApiTest.Rpc.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDTO>> GetAllAsync();
    }
}
