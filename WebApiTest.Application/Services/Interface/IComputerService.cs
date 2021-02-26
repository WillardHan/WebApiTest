using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Application.Models;

namespace WebApiTest.Application.Services
{
    public interface IComputerService
    {
        Task<IEnumerable<ComputerModel>> GetAll();
    }
}
