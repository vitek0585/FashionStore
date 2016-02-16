using System.Data.Entity.ModelConfiguration;
using FashionStore.Domain.Core.Entities.Store;

namespace FashionStore.Infrastructure.Data.Context.Store.Configuration
{
    public class GoodConfiguration:EntityTypeConfiguration<Good>
    {
        public GoodConfiguration()
        {
            ToTable("Good");
            
            HasKey(g => g.GoodId);
            
            Property(e => e.PriceUsd)
               .HasPrecision(18, 0);

            Property(g => g.GoodNameRu).IsRequired();
            Property(g => g.GoodNameEn).IsRequired();

            Property(g => g.DateCreate)
                .HasColumnType("date");

            //HasMany(g => g.ClassificationGoods)
            //    .WithRequired(c => c.Good)
            //    .HasForeignKey(c => c.GoodId)
            //    .WillCascadeOnDelete(true);


            HasMany(g => g.SalePoses)
              .WithOptional(c => c.Good)
              .HasForeignKey(c => c.GoodId)
              .WillCascadeOnDelete(false);
            
            HasMany(g => g.Image)
               .WithRequired(c => c.Good)
               .HasForeignKey(c => c.GoodId)
               .WillCascadeOnDelete(true);
        }
    }
}