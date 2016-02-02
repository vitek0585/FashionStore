using System.Collections.Generic;

namespace FashionStore.Models.Interfaces
{
    

    public interface ICategoryDescModel
    {
        int CategoryId { get; set; }
        string TypeName { get; set; }
        string CategoryName { get; set; }
        string TypeByHref { get; set; }
        IEnumerable<ICategoryDescModel> Childrens { get; set; } 

    }
    
    public interface ICategoryModelBase
    {
        int CategoryId { get; set; }
        string CategoryName { get; set; }
        string TypeByHref { get; set; }

    }
    public interface ITypeCategoryModel<T> where T : ICategoryModelBase
    {
        int TypeId { get; set; }
        string TypeName { get; set; }
        string TypeByHref { get; set; }
        IEnumerable<T> Items { get; set; }
    }
    public interface ICategoryModel : ICategoryModelBase
    {
        int Discount { get; set; }
        string Photo { get; set; }
    }
}