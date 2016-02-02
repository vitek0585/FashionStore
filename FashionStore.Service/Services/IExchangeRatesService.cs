using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Service.Interfaces.Services.Common;

namespace FashionStore.Service.Interfaces.Services
{
    public interface IExchangeRatesService:IEntityService<ExchangeRates>
    {
        decimal ConvertWithCeilingUsdToWithDiscount(decimal value, decimal discount, decimal rate, string convertTo);
        decimal ConvertUsdTo(decimal value,decimal rate, string convertTo);
        decimal ConvertWithCeilingUsdTo(decimal value, decimal rate, string convertTo);
        decimal GetActualRateUsdWithUpdateIfNotExist(bool isUpdate = true, bool getPrev = false);
    }
}