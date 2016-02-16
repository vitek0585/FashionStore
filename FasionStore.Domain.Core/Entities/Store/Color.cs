using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Domain.Core.Entities.Store
{
    [Table("Color")]
    public partial class Color
    {
        public Color()
        {
            ClassificationGoods = new HashSet<ClassificationGood>();
        }

        public int ColorId { get; set; }

        [Required]
        [StringLength(100)]
        public string ColorNameRu { get; set; }
        [Required]
        [StringLength(100)]
        public string ColorNameEn { get; set; }
        public virtual ICollection<ClassificationGood> ClassificationGoods { get; set; }

        public virtual ICollection<SalePos> SalePoses { get; set; }
    }
}
