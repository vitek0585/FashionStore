using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Domain.Core.Entities.Store
{

    public partial class Goods
    {
        public Goods()
        {
            ClassificationGoods = new HashSet<ClassificationGood>();
            SalePoses = new HashSet<SalePos>();
            Image = new HashSet<Image>();
        }

        public int GoodsId { get; set; }
        public string GoodNameRu { get; set; }
        public string GoodNameEn { get; set; }
        public decimal PriceUsd { get; set; }
        public string Articul { get; set; }
        public string DescriptionRu { get; set; }

        public string DescriptionEn { get; set; }

        public int? Discount { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<ClassificationGood> ClassificationGoods { get; set; }
        public virtual ICollection<Image> Image { get; set; }

        public virtual ICollection<SalePos> SalePoses { get; set; }
    }
}
