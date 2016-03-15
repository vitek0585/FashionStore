using System.Threading.Tasks;
using FashionStore.Infrastructure.Data.Identity.Results;

namespace FashionStore.Infrastructure.Data.Identity.Interfaces.Service
{
    public interface IUserIdentityService
    {
        Task<UserInfoIdentity> GetUserInfo(int id);
        int GetUserId();
    }
}