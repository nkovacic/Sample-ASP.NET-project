using Sample.Core.Data.Database.Entities;
using System;
using System.Linq.Expressions;

namespace Sample.Core.Data.Database
{
    public interface IQueryObject<TEntity> where TEntity : Entity
    {
        Expression<Func<TEntity, bool>> Query();
        Expression<Func<TEntity, bool>> And(Expression<Func<TEntity, bool>> query);
        Expression<Func<TEntity, bool>> Or(Expression<Func<TEntity, bool>> query);
        Expression<Func<TEntity, bool>> And(IQueryObject<TEntity> queryObject);
        Expression<Func<TEntity, bool>> Or(IQueryObject<TEntity> queryObject);
    }
}