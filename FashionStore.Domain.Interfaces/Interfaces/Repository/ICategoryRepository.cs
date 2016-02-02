using System.Collections.Generic;
using System.Linq;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository.Common;

namespace FashionStore.Domain.Interfaces.Repository
{
    public interface ICategoryRepository:IGlobalRepository<Category>
    {
        IQueryable<CategoryType> GetCategoryTypes();

        IEnumerable<int> GetCategoryAndChildId(int id);

     


    }
}