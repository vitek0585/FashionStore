using System.ComponentModel.DataAnnotations;

namespace FashionStore.Domain.Core.Entities.Store
{
    public partial class SalePos
    {
        [Key]
        public int SalePosId { get; set; }
        public decimal Price { get; set; }
        public int CountGood { get; set; }

        public int SaleId { get; set; }

        public virtual Sale Sale { get; set; }
        public int? Discount { get; set; }

        public int? GoodId { get; set; }
        public virtual Good Good { get; set; }
        public int? SizeId { get; set; }
        public virtual Size Size { get; set; }
        public int? ColorId { get; set; }
        public virtual Color Color { get; set; }
    }
}
