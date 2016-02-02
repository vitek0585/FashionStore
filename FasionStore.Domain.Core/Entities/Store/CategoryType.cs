using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Domain.Core.Entities.Store
{
    [Table("CategoryType")]
    public partial class CategoryType
    {
        public CategoryType()
        {
            Categories = new HashSet<Category>();
        }
        [Key]
        public int TypeId { get; set; }

        [StringLength(20)]
        public string TypeNameEn { get; set; }
        [StringLength(20)]
        public string TypeNameRu { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
