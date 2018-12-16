using System.Linq;
using System.Threading.Tasks;

namespace GrabNReadApp.Data.Contracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> All();

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        Task<TEntity> GetByIdAsync(params object[] id);

        void Delete(TEntity entity);

        Task<int> SaveChangesAsync();
    }
}
