using System.Data.Entity.ModelConfiguration;
using FashionStore.Domain.Core.Entities.Store;

namespace FashionStore.Infrastructure.Data.Context.Store.Configuration
{
    public class SalePosConfiguration:EntityTypeConfiguration<SalePos>
    {
        public SalePosConfiguration()
        {
            Property(e => e.Price)
                .HasPrecision(18, 0);
        }
         
    }
}