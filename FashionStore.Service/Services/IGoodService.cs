using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Service.Interfaces.Services.Common;

namespace FashionStore.Service.Interfaces.Services
{
    public interface IGoodService : IEntityService<Good>
    {
        TResult GetGood<TResult>(int id, string currentCurrency, string lang);
        IEnumerable<TResult> GetRandomGoods<TResult>(int count, string currentCurrency, string lang);
        IEnumerable<TResult> GetNewGoods<TResult>(int count, string currentCurrency, string lang);
        TResult GetByPage<TResult>(int page, int totalPerPage, int category, string currentCurrency, string lang, 
            Expression<Func<Good, bool>> predicat, string ordering);
        IEnumerable<TResult> GetOrdersById<TResult>(IEnumerable<int> id, string currentCurrency, string lang);
        IEnumerable<TResult> GetGoods<TResult>(IEnumerable<int> ids, string lang);
    }
}