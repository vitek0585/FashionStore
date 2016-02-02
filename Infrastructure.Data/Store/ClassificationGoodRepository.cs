using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infastructure.Data.Repository.Common;
using FashionStore.Infrastructure.Data.Context.Store.Context;

namespace FashionStore.Infastructure.Data.Repository.Store
{
    public class ClassificationGoodRepository : GlobalRepository<ClassificationGood>, IClassificationGoodRepository
    {
        public ClassificationGoodRepository(ShopContext context)
            : base(context)
        {
        }

       

    }
}