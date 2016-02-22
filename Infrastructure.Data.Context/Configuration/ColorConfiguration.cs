using System.Data.Entity.ModelConfiguration;
using FashionStore.Domain.Core.Entities.Store;

namespace FashionStore.Infrastructure.Data.Context.Store.Configuration
{
    public class ColorConfiguration:EntityTypeConfiguration<Color>
    {
        public ColorConfiguration()
        {
            Property(c => c.ColorNameEn).IsRequired().HasMaxLength(100);
            Property(c => c.ColorNameRu).IsRequired().HasMaxLength(100);

        }         
    }
}