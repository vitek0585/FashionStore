using System.ComponentModel.DataAnnotations;

namespace FashionStore.Domain.Core.Entities.Store
{
    public partial class SalePos
    {
        [Key]
        public int SalePosId { get; set; }
        public decimal Price { get; set; }
        public int CountGood { get; set; }

        public int? ClassificationId { get; set; }

        public virtual ClassificationGood ClassificationGood { get; set; }
        public int SaleId { get; set; }

        public virtual Sale Sale { get; set; }
        public int? Discount { get; set; }
    }
}
