using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infrastructure.Data.Context.Store.Context;
using FashionStore.Infrastructure.Data.Repository.Common;

namespace FashionStore.Infrastructure.Data.Repository.Store
{
    public class ClassificationGoodRepository : GlobalRepository<ClassificationGood>, IClassificationGoodRepository
    {
        public ClassificationGoodRepository(ShopContext context)
            : base(context)
        {
        }

       

    }
}