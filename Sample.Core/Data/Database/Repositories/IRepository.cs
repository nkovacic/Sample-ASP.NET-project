using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sample.Core.Data.Database.Entities;
using System.Threading.Tasks;

namespace Sample.Core.Data.Database
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        TEntity Find(params object[] keyValues);
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);
        Task<int> BulkInsertOrUpdateAsync(IEnumerable<TEntity> entities);
        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void InsertOrUpdateGraph(TEntity entity);
        void InsertOrUpdateGraphRange(IEnumerable<TEntity> entity);
        void InsertGraph(TEntity entity);
        void InsertGraphRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        void DeleteGraph(TEntity entity);
        void DeleteGraphRange(IEnumerable<TEntity> entities);
        IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject);
        IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query);
        IQueryFluent<TEntity> Query();
        IQueryable<TEntity> Queryable();
        IRepository<T> GetRepository<T>() where T : Entity;
    }
}