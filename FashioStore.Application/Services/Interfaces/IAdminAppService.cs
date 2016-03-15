using System.Collections.Generic;
using System.Threading.Tasks;
using FashionStore.Service.Interfaces.Services.Common;

namespace FashionStore.Application.Services.Interfaces
{
    public interface IAdminAppService
    {
        Task<TResult> GetGoodsByPageAsync<TResult>(int categoryId, int page, int perPage);
        Task<IEnumerable<TResult>> AllCategoryByTypeAsync<TResult>();
        Task<TResult> FullInfoGoodsAsync<TResult>(int id);
        Task<IEnumerable<TResult>> AllColorsAsync<TResult>();
        Task<IEnumerable<TResult>> AllSizesAsync<TResult>();

    }
}