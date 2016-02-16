using System.Collections.Generic;

namespace FashionStore.Areas.AdminArea.Models
{
    public class ContainerGoodsViewModel
    {
        public IEnumerable<object> Category { get; set; }
        public IEnumerable<object> Colors { get; set; }
        public IEnumerable<object> Sizes { get; set; } 

    }
}