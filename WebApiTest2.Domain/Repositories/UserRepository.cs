using System.Linq;
using WebApiTest2.Domain.Models;
using WebApi.Infrastructure.Repository;

namespace WebApiTest2.Domain.Repositories
{
    public class UserRepository : Repository<User, DatabaseContext>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context)
        { 
        }

        public User FingByCode(string code)
        {
            return context.User.FirstOrDefault(m => m.Code == code);
        }

        public bool IsCodeExist(string code)
        {
            return context.User.Any(m => m.Code == code);
        }
    }
}
