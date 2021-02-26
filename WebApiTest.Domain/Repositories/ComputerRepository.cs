using System.Linq;
using WebApiTest.Domain.Models;
using WebApiTest.Infrastructure.Repository;

namespace WebApiTest.Domain.Repositories
{
    public class ComputerRepository : Repository<Computer, DatabaseContext>, IComputerRepository
    {
        public ComputerRepository(DatabaseContext context) : base(context)
        { 
        }

        public Computer FingByCode(string code)
        {
            return context.Computer.FirstOrDefault(m => m.Code == code);
        }

        public bool IsCodeExist(string code)
        {
            return context.Computer.Any(m => m.Code == code);
        }
    }
}
