using System.Data.Entity.ModelConfiguration;
using FashionStore.Domain.Core.Entities.Store;

namespace FashionStore.Infrastructure.Data.Context.Store.Configuration
{
    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            ToTable("Category");
            HasOptional(x => x.Parent)
             .WithMany(x => x.Children)
             .HasForeignKey(x => x.ParentId)
             .WillCascadeOnDelete(false);

            HasMany(x => x.Goods)
            .WithOptional(g => g.Category)
            .HasForeignKey(g => g.CategoryId);

            Property(c => c.ImagePath).HasMaxLength(100);
        }
    }
}