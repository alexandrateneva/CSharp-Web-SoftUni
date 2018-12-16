using System;
using System.Linq;
using System.Threading.Tasks;
using GrabNReadApp.Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GrabNReadApp.Data
{
    public class DbRepository<TEntity> : IRepository<TEntity>, IDisposable
        where TEntity : class
    {
        private readonly GrabNReadAppContext context;
        private DbSet<TEntity> dbSet;

        public DbRepository(GrabNReadAppContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<TEntity>();
        }

        public Task AddAsync(TEntity entity)
        {
            return this.dbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            var entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.dbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public Task<TEntity> GetByIdAsync(params object[] id)
        {
            return this.dbSet.FindAsync(id);
        }

        public IQueryable<TEntity> All()
        {
            return this.dbSet;
        }

        public void Delete(TEntity entity)
        {
            this.dbSet.Remove(entity);
        }

        public Task<int> SaveChangesAsync()
        {
            return this.context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
