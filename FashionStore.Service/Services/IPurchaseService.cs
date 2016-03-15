using System.Collections.Generic;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Service.Interfaces.Results;

namespace FashionStore.Service.Interfaces.Services
{

    public interface IPurchaseService
    {
        ClassificationGood GetClassification(ClassificationGood good);
        PurchaseResult MakeAnOrder(IEnumerable<ClassificationGood> list,int? userId,
            string userName = null, string phone = null, string email = null);
        IEnumerable<TResult> GetGoodsByCart<TResult>(IEnumerable<ClassificationGood> map, string currentCurrency, string lang);
        IEnumerable<TResult> GetGoodsDetails<TResult>(int id);
    }
}