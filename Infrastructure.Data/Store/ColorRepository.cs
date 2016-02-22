using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infrastructure.Data.Context.Store.Context;
using FashionStore.Infrastructure.Data.Repository.Common;

namespace FashionStore.Infrastructure.Data.Repository.Store
{
    public class ColorRepository:GlobalRepository<Color>,IColorRepository
    {
        public ColorRepository(ShopContext context) : base(context)
        {
        }
    }
}