using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infrastructure.Data.Context.Store.Context;
using FashionStore.Infrastructure.Data.Repository.Common;

namespace FashionStore.Infrastructure.Data.Repository.Store
{
    public class CategoryRepository : GlobalRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ShopContext context)
            : base(context)
        {
        }

        public IQueryable<CategoryType> GetCategoryTypes()
        {
            return _context.Set<CategoryType>();
        }

        public IEnumerable<int> GetCategoryAndChildId(int id)
        {
            SqlParameter param = new SqlParameter("@categoryId", id);
            var str = _context.Database.SqlQuery<string>("GetChildCategoryId @categoryId", param).FirstOrDefault();
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException(string.Format("Not found category by id {0}", id));

            return str.Split(',').Select(n => int.Parse(n, CultureInfo.InvariantCulture));
        }
    }
}