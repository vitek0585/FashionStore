using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Domain.Core.Entities.Store
{
    [Table("ClassificationGood")]
    public partial class ClassificationGood
    {
       
        [Key]
        public int ClassificationId { get; set; }

        public int SizeId { get; set; }

        public int ColorId { get; set; }

        public int GoodId { get; set; }

        public int CountGood { get; set; }

        public virtual Color Color { get; set; }

        public virtual Good Good { get; set; }

        public virtual Size Size { get; set; }

    }
}
