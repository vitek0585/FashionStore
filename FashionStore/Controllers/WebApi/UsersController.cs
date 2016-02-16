using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using FashionStore.Infastructure.Data.Identity.Entities;
using FashionStore.Infastructure.Data.Identity.Interfaces.Service;
using FashionStore.Infastructure.Data.Identity.Manager;

namespace FashionStore.Controllers.WebApi
{
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private IUserAppService _userSvc;
        private byte _totalPerPage = 10;

        public UsersController(IUserAppService userSvc)
        {
            _userSvc = userSvc;
        }

        [Route("ByPage"), HttpGet]
        public async Task<IHttpActionResult> UsersByPage(short page = 1)
        {
            var users = await _userSvc.UsersByPage<dynamic>(page, _totalPerPage);
            return OkOrNoContent<dynamic>(users);
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

        //public async Task<HttpStatusCodeResult> SetRoles(int id, string[] roles)
        //{
        //    User user;
        //    try
        //    {
        //        user = await _userManager.FindByIdAsync(id);
        //        if (user == null || roles == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "User or role not found");
        //        }
        //        var currentRole = await _userManager.GetRolesAsync(id);

        //        if (currentRole.Any())
        //        {
        //            var toDelete = currentRole.Except(roles).ToArray();
        //            var toAdd = roles.Except(currentRole).ToArray();
        //            var result = await _userManager.RemoveFromRolesAsync(id, toDelete);
        //            if (result.Errors.Any())
        //                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "RemoveFromRolesAsync");

        //            result = await _userManager.AddToRolesAsync(id, toAdd);
        //            if (result.Errors.Any())
        //                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "RemoveFromRolesAsync");

        //        }
        //        else
        //        {
        //            var result = await _userManager.AddToRolesAsync(id, roles);
        //            if (result.Errors.Any())
        //                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "AddToRolesAsync");
        //        }


        //    }
        //    catch (Exception e)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, e.Message);

        //    }
        //    return new HttpStatusCodeResult(HttpStatusCode.OK,
        //        string.Format("Roles in user by id {0} was be refreshed", id));
        //}
        #region Helper methods

        [NonAction]
        private IHttpActionResult OkOrNoContent<TResult>(TResult data)
        {
            if (data == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(data);
        }
        [NonAction]
        private IHttpActionResult SuccessOrNot(bool data)
        {
            return data ? StatusCode(HttpStatusCode.NoContent) : StatusCode(HttpStatusCode.BadRequest);
        }
        #endregion
    }
}