using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiTest.Services
{
    public interface IUserService
    {
        Task<List<string>> GetByName(string name);
    }
}
