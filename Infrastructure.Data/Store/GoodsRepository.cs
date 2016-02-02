using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infastructure.Data.Repository.Common;
using FashionStore.Infrastructure.Data.Context.Store.Context;
using LinqKit;

namespace FashionStore.Infastructure.Data.Repository.Store
{
    public class GoodsRepository : GlobalRepository<Good>, IGoodsRepository
    {

        public GoodsRepository(ShopContext context)
            : base(context)
        {

        }

      
        public void UpdateOnlyField(Good good, params Expression<Func<Good, object>>[] expressions)
        {

            _dbSet.Attach(good);
            Array.ForEach(expressions, e => _context.Entry(good).Property(e).IsModified = true);
        }

        public Good GetById(int id)
        {
            var good = _dbSet.Find(id);
            return good;
        }
        public TResult GetById<TResult>(int id, Expression<Func<Good, TResult>> select, params Expression<Func<Good, object>>[] include)
        {
            IQueryable<Good> query = _dbSet;
            var good = include.Aggregate(query, (a, i) => a.Include(i)).AsExpandable().Where(g => g.GoodId == id)
                .Select(select);

            return good.FirstOrDefault();
        }

        public IEnumerable<TResult> SqlQuery<TResult>(string query,params object[] param)
        {
            var result = _context.Database.SqlQuery<TResult>(query, param);
            return result.AsEnumerable();
        }
        
        

      
    }
}