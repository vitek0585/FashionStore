using System.Data.Entity;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Infrastructure.Data.Context.Store.Configuration;

namespace FashionStore.Infrastructure.Data.Context.Store.Context
{

    public partial class ShopContext : DbContext
    {
        public ShopContext()
            : base("name=ShopContext")
        {
        }


        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ClassificationGood> ClassificationGoods { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<CategoryName> CategoryNames { get; set; }
        public virtual DbSet<CategoryType> CategoryTypes { get; set; }
        public virtual DbSet<Good> Goods { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<SalePos> SalePos { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<ExchangeRates> Rates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Configurations.Add(new GoodConfiguration());
            modelBuilder.Configurations.Add(new ClassificationGoodConfiguration());
            modelBuilder.Configurations.Add(new SaleConfiguration());
            modelBuilder.Configurations.Add(new SalePosConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new SizeConfiguration());
            modelBuilder.Configurations.Add(new ColorConfiguration());
            modelBuilder.Configurations.Add(new ImageConfiguration());



        }
    }
}
