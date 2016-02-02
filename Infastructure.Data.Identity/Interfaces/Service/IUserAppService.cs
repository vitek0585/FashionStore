using System.Collections.Generic;
using System.Threading.Tasks;
using FashionStore.Infastructure.Data.Identity.Results;

namespace FashionStore.Infastructure.Data.Identity.Interfaces.Service
{
    public interface IUserAppService
    {
        Task<UserInfoIdentity> GetUserInfoAsync(int? id);
        IEnumerable<TResult> UserSales<TResult>(int? id, int page, int totalPerPage, string currentCurrency, string lang);
        int GetUserId();
    }
}