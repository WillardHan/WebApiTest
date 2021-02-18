using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Services
{
    public interface IConnectionService
    {
        Task<string> GetConnectionString();
    }
}
