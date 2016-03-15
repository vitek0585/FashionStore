using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FashionStore.Domain.Interfaces.Repository.Common
{
    public interface IGlobalRepository
    {
    }
    public interface IGlobalRepository<T> : IGlobalRepository where T : class
    {
        IQueryable<T> GetAll();
        IEnumerable<T> FindBy(Func<T, bool> predicat);

       
        T GetByExpression
            (Expression<Func<T, bool>> expr, params Expression<Func<T, object>>[] include);
        TResult GetByExpressionSelect<TResult>(Expression<Func<T, bool>> expr, Expression<Func<T, TResult>> select,
            params Expression<Func<T, object>>[] include);

        IQueryable<TResult> GetByExpressionSelectList<TResult>
            (Expression<Func<T, bool>> expr, Expression<Func<T, TResult>> select,
                params Expression<Func<T, object>>[] include);
        T Add(T item);
        void Delete(T item);
        void Save();
        void Update(T item);
        TResult DisabledProxy<TResult>() where TResult : IGlobalRepository<T>;
        TResult EnableProxy<TResult>() where TResult : IGlobalRepository<T>;
        int GetCount();

        #region async methods
        Task<T> GetByQueryAsync
            (Expression<Func<T, bool>> expr, params Expression<Func<T, object>>[] include);

        Task<TResult> QueryProjectionAsync<TResult>(Expression<Func<T, bool>> expr, Expression<Func<T, TResult>> select,
            params Expression<Func<T, object>>[] include);

        Task<int> SaveAsync();

        #endregion

    }
}