using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Domain.Core.Entities.Store
{
    [Table("Sale")]
    public partial class Sale
    {
        public Sale()
        {
            SalePos = new HashSet<SalePos>();
        }

        public int SaleId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DateSale { get; set; }

        public int? UserId { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }
        public virtual ICollection<SalePos> SalePos { get; set; }
    }
}
