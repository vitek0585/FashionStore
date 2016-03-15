using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FashionStore.Domain.Core.Entities.Store;

namespace FashionStore.Infrastructure.Data.Context.Store.Configuration
{
    public class SizeConfiguration:EntityTypeConfiguration<Size>
    {
        public SizeConfiguration()
        {
            
            ToTable("Size");
            Property(s => s.SizeName).IsRequired().HasMaxLength(20);
            HasKey(s => s.SizeId)
                .Property(s => s.SizeId);
        }
    }
}