namespace OnlineTechStore.Data.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IDbRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> All();

        Task AddAsync(TEntity entity);

        void Delete(TEntity entity);

        void UpdateStudent(TEntity entity);

        Task<int> SaveChangesAsync();
    }
}
