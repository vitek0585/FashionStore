using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Domain.Core.Entities.Store
{
    [Table("CategoryName")]
    public partial class CategoryName
    {
        public CategoryName()
        {
            Categories = new HashSet<Category>();
        }
        [Key]
        public int NameId { get; set; }
    

        [StringLength(100)]
        public string CategoryNameEn { get; set; }

        [StringLength(100)]
        public string CategoryNameRu { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
