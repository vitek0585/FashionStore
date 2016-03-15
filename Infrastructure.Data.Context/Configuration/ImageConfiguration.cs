using System.CodeDom;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FashionStore.Domain.Core.Entities.Store;

namespace FashionStore.Infrastructure.Data.Context.Store.Configuration
{
    public class ImageConfiguration:EntityTypeConfiguration<Image>
    {
        public ImageConfiguration()
        {
            HasKey(p => p.ImageId).Property(i => i.ImageId);//.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            
        }
    }
}