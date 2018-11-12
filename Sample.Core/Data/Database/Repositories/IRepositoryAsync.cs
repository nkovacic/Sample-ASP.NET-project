using System.Threading;
using System.Threading.Tasks;
using Sample.Core.Data.Database.Entities;

namespace Sample.Core.Data.Database
{
    public interface IRepositoryAsync<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> FindAsync(params object[] keyValues);
        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
        Task<bool> DeleteAsync(params object[] keyValues);
        Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues);
    }
}