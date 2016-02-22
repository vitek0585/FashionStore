using System.Collections.Generic;
using System.Threading.Tasks;
using FashionStore.Infrastructure.Data.Identity.Results;
using Microsoft.AspNet.Identity;

namespace FashionStore.Infrastructure.Data.Identity.Interfaces.Service
{
    public interface IUserAppService
    {
        Task<UserInfoIdentity> GetUserInfoAsync(int? id);
        IEnumerable<TResult> UserSales<TResult>(int? id, int page, int totalPerPage, string currentCurrency, string lang);
        int GetUserId();

        Task<TResult> UsersByPage<TResult>(int page, int perPage);
        Task<IdentityResult> UpdateRoles(int id, string[] roles);

    }
}