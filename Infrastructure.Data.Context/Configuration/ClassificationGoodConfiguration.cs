using System.Data.Entity.ModelConfiguration;
using FashionStore.Domain.Core.Entities.Store;

namespace FashionStore.Infrastructure.Data.Context.Store.Configuration
{
    public class ClassificationGoodConfiguration:EntityTypeConfiguration<ClassificationGood>
    {
        public ClassificationGoodConfiguration()
        {
            
            HasMany(e => e.SalePoses)
             .WithOptional(s => s.ClassificationGood)
             .HasForeignKey(s => s.ClassificationId)
             .WillCascadeOnDelete(false);
        }
 
    }
}