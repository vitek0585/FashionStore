using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Domain.Core.Entities.Store
{
    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
            Goods = new HashSet<Good>();
        }
    
        public int CategoryId { get; set; }
        [StringLength(300)]
        public string ImagePath { get; set; }
        public int? ParentId { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }
        public int? TypeId { get; set; }
        public int? NameId { get; set; }

        public virtual CategoryName Name { get; set; }
        public virtual CategoryType Type { get; set; }
        public virtual ICollection<Good> Goods { get; set; }
    }
}
