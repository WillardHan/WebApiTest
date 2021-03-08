using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest2.Application.Models;

namespace WebApiTest2.Application.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetAll();
    }
}
