using System;
using System.Linq;
using WebApiTest.Domain.Models;
using WebApiTest.Infrastructure.Repository;

namespace WebApiTest.Domain.Repositories
{
    public class CompanyRepository : Repository<Company, DatabaseContext>, ICompanyRepository
    {
        public CompanyRepository(DatabaseContext context) : base(context)
        { 
        }

        public override Company Get(Guid Id)
        {
            var entity = context.Company.Find(Id);
            if (entity != null)
            {
                context.Entry(entity).Collection(m => m.Departments).Load();
            }
            return entity;
        }

        public Company FingByCode(string code)
        {
            return context.Company.FirstOrDefault(m => m.Code == code);
        }

        public bool IsCodeExist(string code)
        {
            return context.Company.Any(m => m.Code == code);
        }
    }
}
