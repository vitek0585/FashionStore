using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infrastructure.Data.Context.Store.Context;
using FashionStore.Infrastructure.Data.Repository.Common;

namespace FashionStore.Infrastructure.Data.Repository.Store
{
    public class SizeRepository : GlobalRepository<Size>, ISizeRepository
    {
        public SizeRepository(ShopContext context)
            : base(context)
        {
        }
    }
}