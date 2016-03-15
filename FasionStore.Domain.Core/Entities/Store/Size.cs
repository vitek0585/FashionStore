using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Domain.Core.Entities.Store
{

    public partial class Size
    {
       
        public Size()
        {
            ClassificationGoods = new HashSet<ClassificationGood>();
        }

        public int SizeId { get; set; }

    
        public string SizeName { get; set; }

   
        public virtual ICollection<ClassificationGood> ClassificationGoods { get; set; }

        public virtual ICollection<SalePos> SalePoses { get; set; }
    }
}
