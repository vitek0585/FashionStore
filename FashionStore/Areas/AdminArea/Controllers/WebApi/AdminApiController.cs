using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using FashionStore.Application.Bootstrapper.InversionOfControl;
using FashionStore.Application.Services.Interfaces;
using FashionStore.Core.Filter.CSRF;
using FashionStore.Infastructure.Data.Identity.Interfaces.Service;
using WebLogger.Abstract.Interface;

namespace FashionStore.Areas.AdminArea.Controllers.WebApi
{

    //[Authorize(Roles = "Admin,Manager")]
    [ValidateAntiForgeryHttp]
    [RoutePrefix("api/Admin")]
    public class AdminApiController : ApiController
    {
        IAdminAppService _admin;
        private IUserAppService _userSvc;
        private ILogWriter<string> _logWriter;
        private int _perPage = 10;
        public AdminApiController(IAdminAppService admin, IUserAppService userSvc, ILogWriter<string> logWriter)
        {
            _admin = admin;
            _userSvc = userSvc;
            _logWriter = logWriter;
        }

        #region goods view
        [Route("GoodsByPage"), HttpGet]
        public async Task<IHttpActionResult> GetGoodsByPage(int page, int category)
        {
            var data = await _admin.GetGoodsByPageAsync<dynamic>(category, page, _perPage);
            return OkOrNoContent<dynamic>(data);
        }

        [Route("GoodsFullInfo"), HttpGet]
        public async Task<IHttpActionResult> FullInfo(int id)
        {
            var data = await _admin.FullInfoGoodsAsync<dynamic>(id);
            return OkOrNoContent<dynamic>(data);
        }

        #endregion

        #region log
        [Route("Log"), HttpGet]
        public IHttpActionResult Log(int page)
        {
            var data = IoC.Resolve<ILogReader>().LogReadPage(page, _perPage);
            return OkOrNoContent<dynamic>(data);
        }
        [Route("LogDelete"), HttpDelete]
        public IHttpActionResult LogDelete(int id)
        {
            var isSuccess = IoC.Resolve<ILog>().Remove(id);
            return SuccessOrNot(isSuccess);
        }
        #endregion

        #region Users

        [Route("UserByPage"), HttpGet]
        public async Task<IHttpActionResult> UsersByPage(short page = 1)
        {
            var users = await _userSvc.UsersByPage<dynamic>(page, _perPage);
            return OkOrNoContent<dynamic>(users);
        }
        [Route("UserUpdateRole"), HttpPut]
        public async Task<HttpResponseMessage> AddOrUpdateRoles(int id,string[] roles)
        {
            try
            {
                var result = await _userSvc.UpdateRoles(id, roles);
                if (result.Errors.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result.Errors);
                }
            }
            catch (Exception e)
            {
                _logWriter.LogWriteError("Add Or Update Roles", e);
                return Request.CreateResponse(HttpStatusCode.BadRequest,e.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion
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