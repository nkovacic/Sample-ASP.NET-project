using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Sample.Core.Data.Database.Entities;

namespace Sample.Core.Data.Database
{
    public interface IQueryFluent<TEntity> where TEntity : Entity
    {
        Task<bool> AnyAsync();
        Task<int> CountAsync();
        Task<TEntity> FirstOrDefaultAsync();
        IQueryFluent<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        IQueryFluent<TEntity> Include(Expression<Func<TEntity, object>> expression);
        IQueryFluent<TEntity> Include(string dotNotationExpression);
        Task<IEnumerable<TEntity>> SelectPageAsync(int page, int pageSize);
        IQueryable<TEntity> SelectPage(int page, int pageSize);
        IQueryable<TEntity> SelectPage(int page, int pageSize, out int totalCount);
        IQueryable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector = null);
        IQueryable<TEntity> Select();
        Task<IEnumerable<TEntity>> SelectAsync();
        Task<IEnumerable<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector = null);
        IQueryable<TEntity> SqlQuery(string query, params object[] parameters);
    }
}