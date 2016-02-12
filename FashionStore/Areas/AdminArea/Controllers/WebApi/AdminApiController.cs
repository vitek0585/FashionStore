using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using FashionStore.Application.Bootstrapper.InversionOfControl;
using FashionStore.Application.Services.Interfaces;
using WebLogger.Abstract.Interface;

namespace FashionStore.Areas.AdminArea.Controllers.WebApi
{
    [RoutePrefix("api/Admin/Goods")]
    public class AdminApiController : ApiController
    {
        IAdminAppService _admin;
        private int _perPage = 10;
        public AdminApiController(IAdminAppService admin)
        {
            _admin = admin;
        }

        #region goods view

        [Route("ByPage"), HttpGet]
        public async Task<IHttpActionResult> GetGoodsByPage(int page, int category)
        {
            var data = await _admin.GetGoodsByPageAsync<dynamic>(category, page, _perPage);
            return OkOrNoContent<dynamic>(data);
        }

        [Route("FullInfo"), HttpGet]
        public async Task<IHttpActionResult> FullInfo(int id)
        {
            var data = await _admin.FullInfoGoodsAsync<dynamic>(id);
            return OkOrNoContent<dynamic>(data);
        }

        #endregion


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