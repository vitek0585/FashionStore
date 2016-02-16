using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Utils.Custom_Validation;

namespace FashionStore.Areas.AdminArea.Models
{
    public class GoodsViewModel
    {
        public GoodsViewModel()
        {
            ClassificationGoods = new HashSet<ClassificationGood>();
        }
        public int GoodId { get; set; }
        [Required(ErrorMessage = "Good Name Ru is required")]
        public string GoodNameRu { get; set; }
        [Required(ErrorMessage = "Good Name En is required")]
        public string GoodNameEn { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Price does not must be less 0")]
        public decimal PriceUsd { get; set; }
    
        public string DescriptionRu { get; set; }
        public string DescriptionEn { get; set; }
        [Range(0, 99, ErrorMessage = "Discount does not must be less 0 and more 99 %")]
        public int? Discount { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int? CategoryId { get; set; }
   
        [ClassificationGoodValidator(ErrorMessage = "Elements color and size of the classification goods is not unique")]
        public virtual ICollection<ClassificationGood> ClassificationGoods { get; set; }
    
      
    }
}