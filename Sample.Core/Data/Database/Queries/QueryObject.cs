using System;
using System.Linq.Expressions;
using LinqKit;
using Sample.Core.Data.Database.Entities;

namespace Sample.Core.Data.Database
{
    public abstract class QueryObject<TEntity> : IQueryObject<TEntity> where TEntity : Entity
    {
        private Expression<Func<TEntity, bool>> _query;

        public virtual bool HasQuery()
        {
            return _query != null;
        }

        public Expression<Func<TEntity, bool>> GetQueryOrDefault()
        {
            return HasQuery() ? _query : And(entity => true);
        }

        public virtual Expression<Func<TEntity, bool>> Query()
        {
            return _query;
        }

        public Expression<Func<TEntity, bool>> And(Expression<Func<TEntity, bool>> query)
        {
            return _query = _query == null ? query : _query.And(query.Expand());
        }

        public Expression<Func<TEntity, bool>> Or(Expression<Func<TEntity, bool>> query)
        {
            return _query = _query == null ? query : _query.Or(query.Expand());
        }

        public Expression<Func<TEntity, bool>> And(IQueryObject<TEntity> queryObject)
        {
            return And(queryObject.Query());
        }

        public Expression<Func<TEntity, bool>> Or(IQueryObject<TEntity> queryObject)
        {
            return Or(queryObject.Query());
        }

        public virtual QueryObject<TEntity> Clear()
        {
            _query = null;

            return this;
        }
    }
}