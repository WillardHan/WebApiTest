using System;
using System.Threading.Tasks;

namespace WebApiTest.Infrastructure.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(Guid id);

        TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);

        void Delete(int id);

        Task<int> SaveChangesAsync();
    }
}
