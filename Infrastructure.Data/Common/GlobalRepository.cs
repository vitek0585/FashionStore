using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FashionStore.Domain.Interfaces.Repository.Common;
using LinqKit;

namespace FashionStore.Infastructure.Data.Repository.Common
{
    public class GlobalRepository<T> : IGlobalRepository<T> where T : class
    {
        protected DbContext _context;
        protected IDbSet<T> _dbSet;

        public GlobalRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public TResult GetByExpressionSelect<TResult>
           (Expression<Func<T, bool>> expr, Expression<Func<T, TResult>> select, params Expression<Func<T, object>>[] include)
        {
            IQueryable<T> query = _dbSet;
            var item = include.Aggregate(query, (a, i) => a.Include(i)).AsExpandable().Where(expr).
                Select(select);
            return item.FirstOrDefault();
        }

        public IQueryable<TResult> GetByExpressionSelectList<TResult>
            (Expression<Func<T, bool>> expr, Expression<Func<T, TResult>> select, params Expression<Func<T, object>>[] include)
        {
            IQueryable<T> query = _dbSet;
            var item = include.Aggregate(query, (a, i) => a.Include(i)).AsExpandable().Where(expr).Select(select);
            return item;
        }

        public T GetByExpression
          (Expression<Func<T, bool>> expr, params Expression<Func<T, object>>[] include)
        {
            IQueryable<T> query = _dbSet;
            var item = include.Aggregate(query, (a, i) => a.Include(i)).AsExpandable().Where(expr);
            return item.FirstOrDefault();
        }
        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }
      
        public IEnumerable<T> FindBy(Func<T, bool> predicat)
        {
            return _dbSet.Where(predicat);
        }

        public T Add(T item)
        {
            return _dbSet.Add(item);
        }

        public void Delete(T item)
        {
            _dbSet.Remove(item);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T item)
        {
            _dbSet.AddOrUpdate(item);
        }

        public TResult DisabledProxy<TResult>() where TResult : IGlobalRepository<T>
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return (TResult)(this as IGlobalRepository<T>);
        }
        public TResult EnableProxy<TResult>() where TResult : IGlobalRepository<T>
        {
            _context.Configuration.ProxyCreationEnabled = true;
            return (TResult)(this as IGlobalRepository<T>);
        }
        public int GetCount()
        {
            return _dbSet.Count();
        }

#region async methods
        public Task<T> GetByQueryAsync(Expression<Func<T, bool>> expr, params Expression<Func<T, object>>[] include)
        {
            IQueryable<T> query = _dbSet;
            var item = include.Aggregate(query, (a, i) => a.Include(i)).AsExpandable().Where(expr);
            return item.FirstOrDefaultAsync();
        }

        public Task<TResult> QueryProjectionAsync<TResult>(Expression<Func<T, bool>> expr, Expression<Func<T, TResult>> @select, params Expression<Func<T, object>>[] include)
        {
            IQueryable<T> query = _dbSet;
            var item = include.Aggregate(query, (a, i) => a.Include(i)).AsExpandable().Where(expr).
                Select(select);
            return item.FirstOrDefaultAsync();
        }
        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
#endregion
    }
}