using System.Collections.Generic;

namespace FashionStore.Models.Interfaces
{
    public interface IGoodModel
    {
        int GoodId { get; set; }
        int CategoryId { get; set; }
        string CategoryName { get; set; }
        string TypeName { get; set; }
        string GoodName { get; set; }
        int GoodCount { get; set; }
        int Discount { get; set; }
        IEnumerable<string> Photos { get; set; }
        decimal PriceUsd { get; set; }
 
        decimal PriceWithDiscount { get; set; }
           
         IEnumerable<ITypeGoodModel> Types { get; set; }

        string Description { get; set; }
               
        
    }

    public interface ITypeGoodModel
    {
        int ColorId { get; set; }
        string ColorName { get; set; }
        int SizeId { get; set; }
        string SizeName { get; set; }
        int CountGood { get; set; }
        
    }
}