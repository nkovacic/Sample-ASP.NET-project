using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Sample.Core.Data.Database.Entities;
using Sample.Core.ViewModels;

namespace Sample.Core.Data.Database
{
    public sealed class QueryFluent<TEntity> : IQueryFluent<TEntity> where TEntity : Entity
    {
        #region Private Fields
        private readonly Expression<Func<TEntity, bool>> _expression;
        private readonly List<Expression<Func<TEntity, object>>> _includes;
        private readonly List<string> _includesDotNotation;
        private readonly Repository<TEntity> _repository;
        private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> _orderBy;
        #endregion Private Fields

        #region Constructors
        public QueryFluent(Repository<TEntity> repository)
        {
            _repository = repository;
            _includes = new List<Expression<Func<TEntity, object>>>();
            _includesDotNotation = new List<string>();
        }

        public QueryFluent(Repository<TEntity> repository, IQueryObject<TEntity> queryObject) : this(repository) { _expression = queryObject.Query(); }

        public QueryFluent(Repository<TEntity> repository, Expression<Func<TEntity, bool>> expression) : this(repository) { _expression = expression; }
        #endregion Constructors

        public async Task<bool> AnyAsync()
        {
            return await Select().AnyAsync();
        }

        public async Task<int> CountAsync()
        {
            return await Select().CountAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync()
        {
            return await Select().FirstOrDefaultAsync();
        }

        public IQueryFluent<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            _orderBy = orderBy;
            return this;
        }

        public IQueryFluent<TEntity> Include(string dotNotationExpression)
        {
            _includesDotNotation.Add(dotNotationExpression);

            return this;
        }

        public IQueryFluent<TEntity> Include(Expression<Func<TEntity, object>> expression)
        {
            _includes.Add(expression);
            return this;
        }

        public async Task<IEnumerable<TEntity>> SelectPageAsync(int page, int pageSize)
        {
            return await _repository.SelectAsync(_expression, _orderBy, _includes, _includesDotNotation, page, pageSize);
        }

        public async Task<PaginationViewModel<TEntity>> SelectPageAsync(int page, int pageSize, bool includeTotalCount)
        {
            var paginationViewModel = new PaginationViewModel<TEntity>();

            paginationViewModel.SetPaginationState(-1, page, pageSize);

            if (includeTotalCount)
            {
                paginationViewModel.Pagination.Count = await _repository.Select(_expression).CountAsync();
            }

            if (paginationViewModel.Pagination.Count > 0)
            {
                var entities = await _repository.SelectAsync(_expression, _orderBy, _includes, _includesDotNotation, page, pageSize);

                paginationViewModel.AddItems(entities);
            }

            return paginationViewModel;
        }

        public IQueryable<TEntity> SelectPage(int page, int pageSize)
        {
            return _repository.Select(_expression, _orderBy, _includes, _includesDotNotation, page, pageSize);
        }

        public IQueryable<TEntity> SelectPage(int page, int pageSize, out int totalCount)
        {
            totalCount = _repository.Select(_expression).Count();
            return _repository.Select(_expression, _orderBy, _includes, _includesDotNotation, page, pageSize);
        }

        public IQueryable<TEntity> Select() { return _repository.Select(_expression, _orderBy, _includes, _includesDotNotation); }

        public IQueryable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector) { return _repository.Select(_expression, _orderBy, _includes, _includesDotNotation).Select(selector); }

        public async Task<IEnumerable<TEntity>> SelectAsync() { return await _repository.SelectAsync(_expression, _orderBy, _includes, _includesDotNotation); }

        public async Task<IEnumerable<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector = null)
        {
            return await Select(selector).ToListAsync();
        }

        public IQueryable<TEntity> SqlQuery(string query, params object[] parameters) { return _repository.SelectQuery(query, parameters).AsQueryable(); }
    }
}