namespace OnlineTechStore.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using OnlineTechStore.Data.Contracts;

    public class DbRepository<TEntity> : IDbRepository<TEntity>, IDisposable
        where TEntity : class
    {
        private readonly TechStoreContext context;
        private readonly DbSet<TEntity> dbSet;

        public DbRepository(TechStoreContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<TEntity>();
        }

        public Task AddAsync(TEntity entity)
        {
            return this.dbSet.AddAsync(entity);
        }

        public IQueryable<TEntity> All()
        {
            return this.dbSet;
        }

        public void Delete(TEntity entity)
        {
            this.dbSet.Remove(entity);
        }

        public void UpdateStudent(TEntity entity)
        {
            this.dbSet.Update(entity);
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
