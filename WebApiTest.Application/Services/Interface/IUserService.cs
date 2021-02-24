using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiTest.Application.Services
{
    public interface IUserService
    {
        Task<List<string>> GetByName(string name, CancellationToken cancellationToken);
    }
}
