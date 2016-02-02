using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository.Common;

namespace FashionStore.Domain.Interfaces.Repository
{
    public interface IClassificationGoodRepository:IGlobalRepository<ClassificationGood>
    {
        //ClassificationGood GetByClassification(ClassificationGood good);

    }
}