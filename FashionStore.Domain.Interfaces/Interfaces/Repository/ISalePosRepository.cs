using System.Collections.Generic;
using FashionStore.Domain.Core.Entities.Store;

namespace FashionStore.Domain.Interfaces.Repository
{
    public interface ISalePosRepository
    {
       
        void Add(IEnumerable<SalePos> enumerable, Sale sale,string error);
    }
}