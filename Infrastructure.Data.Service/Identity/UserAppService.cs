using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using AutoMapper;
using FashionStore.Infastructure.Data.Identity.Entities;
using FashionStore.Infastructure.Data.Identity.Interfaces.Service;
using FashionStore.Infastructure.Data.Identity.Manager;
using FashionStore.Infastructure.Data.Identity.Results;
using FashionStore.Infastructure.Data.Service.Identity.Common;
using FashionStore.Service.Interfaces.Services;
using FashionStore.Service.Interfaces.UoW;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace FashionStore.Infastructure.Data.Service.Identity
{
    public class UserAppService : AccountGlobalService, IUserAppService
    {
        private ISaleService _saleService;
        private IUnitOfWorkIdentity _unitIdentity;
        public UserAppService(RoleManager roleManager, UserManager userManager, SignInManager
            singInManager, IAuthenticationManager authentication, ISaleService saleService, IUnitOfWorkIdentity unitIdentity) :
            base(roleManager, userManager, singInManager, authentication)
        {
            _unitIdentity = unitIdentity;
            _saleService = saleService;
        }

        public int GetUserId()
        {
            if (!_authentication.User.Identity.IsAuthenticated)
                throw new AuthenticationException("User did not authenticate");

            return _authentication.User.Identity.GetUserId<int>();
        }
        public async Task<UserInfoIdentity> GetUserInfoAsync(int? id)
        {
            if (!id.HasValue)
                id = GetUserId();

            var userInfo = new UserInfoIdentity();
            var user = await _userManager.FindByIdAsync(id.Value);

            userInfo.HasPassword = user.PasswordHash != null;
            userInfo.PhoneNumber = user.PhoneNumber;
            userInfo.Logins = user.Logins;
            userInfo.UserName = user.UserName;
            userInfo.Email = user.Email;

            return userInfo;
        }
        public IEnumerable<TResult> UserSales<TResult>(int? id, int page, int totalPerPage, string currentCurrency, string lang)
        {
            if (!id.HasValue)
                id = GetUserId();

            return _saleService.SaleByPage<TResult>(id.Value, page, totalPerPage, currentCurrency, lang);
        }
        public async Task<TResult> UsersByPage<TResult>(int page, int perPage)
        {
            var skip = (page - 1) * perPage;

            var totalCount = await _userManager.Users.CountAsync();
            var users = (await _userManager.Users.OrderBy(u => u.Id)
                .Skip(() => skip).Take(() => perPage)
                .Select(u => new
                {

                    u.Id,
                    roles = u.Roles.Select(r => new
                    {
                        id = r.RoleId,
                        r.Role.Name

                    }),
                    u.Email,
                    u.PhoneNumber,
                    u.UserName

                }).ToListAsync())
                .GroupBy(u => new
                {
                    totalPagesCount = Math.Ceiling((double)totalCount / perPage)
                })
                .Select(g => new
                {
                    g.Key.totalPagesCount,
                    users = g.Select(u => new
                    {
                        u.Id,
                        u.roles,
                        u.Email,
                        u.PhoneNumber,
                        u.UserName
                    })
                }).FirstOrDefault();

            if (users == null)
            {
                return default(TResult);
            }

            return Mapper.DynamicMap<TResult>(users);
        }

        //public async Task<HttpResponseMessage> RemoveUser(short id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user != null)
        //    {
        //        await _userManager.DeleteAsync(user);
        //        return StatusCode(HttpStatusCode.BadRequest);
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest);

        //    }

        //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest,
        //        string.Format("User by id {0} was not found", id));

        //}

        public async Task<IdentityResult> UpdateRoles(int id, string[] roles)
        {
            try
            {
                _unitIdentity.StartTransaction();
                var currentRole = await _userManager.GetRolesAsync(id);
                IdentityResult result;
                if (currentRole.Any())
                {
                    var toDelete = currentRole.Except(roles).ToArray();
                    var toAdd = roles.Except(currentRole).ToArray();
                    result = await _userManager.RemoveFromRolesAsync(id, toDelete);
                    if (result.Errors.Any())
                    {
                        _unitIdentity.Rollback();
                        return result;
                    }

                    result = await _userManager.AddToRolesAsync(id, toAdd);
                }
                else
                {
                    result = await _userManager.AddToRolesAsync(id, roles);
                }

                if (result.Errors.Any())
                {
                    _unitIdentity.Rollback();
                    return result;
                }
                _unitIdentity.Commit();
                return result;

            }
            catch (Exception e)
            {
                _unitIdentity.Rollback();
                throw e;
            }

        }

        #region additional methods
        private IEnumerable<TResult> DynamicMap<TResult>(IEnumerable<dynamic> data)
        {
            return Mapper.DynamicMap<IEnumerable<TResult>>(data);
        }
        #endregion

    }
}