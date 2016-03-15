using System.Collections.Generic;

namespace FashionStore.Models.Interfaces
{
    public interface IFilterModel
    {
        IEnumerable<IColor> Colors { get; set; }
        IEnumerable<ISize> Sizes { get; set; }
        int Min { get; set; }
        int Max { get; set; }
        string ExchangeRates { get; set; }
        
    }

    public interface IInfoShort
    {
        int Id { get; set; }
        string Name { get; set; }
    }
    public interface IColor : IInfoShort
    {
        string Color { get; set; }
    }
    public interface ISize : IInfoShort
    {
    }
}