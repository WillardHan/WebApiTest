using Microsoft.EntityFrameworkCore;
using WebApiTest2.Domain.Models;

namespace WebApiTest2.Domain
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options)
        {
        
        }

        public DbSet<User> User { get; set; }

    }
}
