using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Domain.Core.Entities.Store
{
     [Table("Image")]
    public class Image
    {
        public int ImageId { get; set; }
        public string ImagePath { get; set; }
        public int GoodId { get; set; }
        public virtual Good Good { get; set; }
    }
}