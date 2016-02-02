using System.Collections.Generic;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Service.Interfaces.Services.Common;

namespace FashionStore.Service.Interfaces.Services
{
    public interface ISaleService : IEntityService<Sale>
    {
        IEnumerable<TResult> SaleByPage<TResult>(int id, int page, int totalPerPage, string currentCurrency, string lang);
    }
}