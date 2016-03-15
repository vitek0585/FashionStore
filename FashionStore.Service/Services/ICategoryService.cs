using System.Collections.Generic;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Service.Interfaces.Services.Common;

namespace FashionStore.Service.Interfaces.Services
{
    public interface ICategoryService : IEntityService<Category>
    {
        IEnumerable<TResult> GetRandom<TResult>(int count, string lang) where TResult : class;
        IEnumerable<TResult> AllCategory<TResult>(string lang) where TResult : class;
        TResult GetCategoriesByType<TResult>(string type, string getCurrentLanguage) where TResult : class;
        TResult GetCategoriesSale<TResult>(string type, string getCurrentLanguage, int discount) where TResult : class;
        TResult GetCategoryByCulture<TResult>(string type, int id, string lang);
        int? GetTypeIdByName(string type);
        TResult GetInformationAboutCategory<TResult>(int category, string currentCurrency, string lang);
        IEnumerable<TResult> CategoryAndChild<TResult>(int id, string lang) where TResult : class;

    }
}