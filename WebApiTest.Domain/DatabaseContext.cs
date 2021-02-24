using Microsoft.EntityFrameworkCore;
using WebApiTest.Domain.Models;

namespace WebApiTest.Domain
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options)
        {
        
        }

        public DbSet<Company> Company { get; set; }

    }
}
