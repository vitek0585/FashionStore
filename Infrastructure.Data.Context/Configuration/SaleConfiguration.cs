using System.Data.Entity.ModelConfiguration;
using FashionStore.Domain.Core.Entities.Store;

namespace FashionStore.Infrastructure.Data.Context.Store.Configuration
{
    public class SaleConfiguration:EntityTypeConfiguration<Sale>
    {
        public SaleConfiguration()
        {


            HasMany(e => e.SalePos)
                .WithRequired(e => e.Sale)
                .HasForeignKey(e => e.SaleId)
                .WillCascadeOnDelete(false);
        } 
    }
}