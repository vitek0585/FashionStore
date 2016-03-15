using System.Data.SqlClient;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infrastructure.Data.Context.Store.Context;
using FashionStore.Infrastructure.Data.Repository.Common;

namespace FashionStore.Infrastructure.Data.Repository.Store
{
    public class ImageRepository : GlobalRepository<Image>, IImageRepository
    {
        public ImageRepository(ShopContext context)
            : base(context)
        {

        }

        public bool AddWithoutId(Image img)
        {
            return _context.Database.ExecuteSqlCommand("insert_photo @path,@id",
                new SqlParameter("@path", img.ImagePath), new SqlParameter("@id", img.GoodId)) > 0;
        }

    }
}