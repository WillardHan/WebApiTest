using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace WebApi.Infrastructure.Repository
{
    public class Repository<TEntity, TDbContext> : IRepository<TEntity>
        where TEntity : class 
        where TDbContext : DbContext
    {
        protected readonly TDbContext context;
        public Repository(TDbContext context)
        {
            this.context = context;
        }

        public virtual TEntity Get(Guid Id)
        {
            return context.Set<TEntity>().Find(Id);
        }

        public TEntity Add(TEntity entity)
        {
            return context.Set<TEntity>().Add(entity).Entity;
        }

        public TEntity Update(TEntity entity)
        {
            return context.Set<TEntity>().Update(entity).Entity;
        }

        public void Delete(int id)
        {
            var entity = context.Set<TEntity>().Find(id);
            if (entity != null)
            {
                context.Set<TEntity>().Remove(entity);
            }
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
