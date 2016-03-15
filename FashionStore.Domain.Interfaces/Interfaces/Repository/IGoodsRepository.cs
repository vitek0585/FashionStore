using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository.Common;

namespace FashionStore.Domain.Interfaces.Repository
{
    public interface IGoodsRepository : IGlobalRepository<Good>
    {
       
        void UpdateOnlyField(Good good, params Expression<Func<Good, object>>[] expressions);
        Good GetById(int id);
        Good GetById(int id, params Expression<Func<Good, object>>[] include);
        TResult GetById<TResult>(int id, Expression<Func<Good, TResult>> select, params Expression<Func<Good, object>>[] include);
        int GetCount();

        IEnumerable<TResult> SqlQuery<TResult>(string query, params object[] param);
    }
}