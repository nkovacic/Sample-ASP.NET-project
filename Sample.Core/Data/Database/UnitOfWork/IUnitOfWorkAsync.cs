using System;
using System.Threading;
using System.Threading.Tasks;
using Sample.Core.Data.Database.Entities;

namespace Sample.Core.Data.Database
{
    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : Entity;
    }
}