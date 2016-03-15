using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using AutoMapper;
using FashionStore.Areas.AdminArea.Models;
using FashionStore.Core.AppValue;
using FashionStore.Core.Binder;
using FashionStore.Core.Filter.ModelValidate;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Models.MediaFormatter;
using FashionStore.Service.Interfaces.Services;
using WebCookie.Interfaces;
using WebLogger.Abstract.Interface;
using WebLogger.Abstract.Interface.Sql;

namespace FashionStore.Controllers.WebApi
{
    [RoutePrefix("api/Good")]
    public class GoodController : ApiController
    {

        private IGoodService _goodService;
        private ILogWriterSql _log;
        private ICookieConsumer _storage;
        private byte _totalPerPage = 9;

        public GoodController(ICookieConsumer storage, IGoodService goodService, ILogWriterSql log)
        {
            _storage = storage;
            _goodService = goodService;
            _log = log;
        }
        [HttpGet]
        [Route("GetGood")]
        public IHttpActionResult GetGood([FromUri]int id)
        {

            try
            {
                var data = _goodService.GetGood<dynamic>(id, GetCurrentCurrency(), GetCurrentLanguage());
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }


        [HttpGet]
        [Route("RandomGood")]
        public IHttpActionResult RandomGood(int count = 4)
        {


            try
            {
                var data = _goodService.GetRandomGoods<dynamic>(count, GetCurrentCurrency(), GetCurrentLanguage());
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpGet]
        [Route("NewGoods")]
        public IHttpActionResult NewGoods()
        {
            try
            {
                int newCount = 4;
                var data = _goodService.GetNewGoods<dynamic>(newCount, GetCurrentCurrency(), GetCurrentLanguage());

                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }
        [Route("GetByPage"), HttpGet]
        public IHttpActionResult GetByPage(short category, int page,
            [ModelBinder(typeof(HttpFilterBinder))]Expression<Func<Good, bool>> pr,
            [ModelBinder(typeof(HttpOrderBinder))]string sort = null)
        {

            try
            {
                var data = _goodService.GetByPage<dynamic>(page, _totalPerPage, category,
                     GetCurrentCurrency(), GetCurrentLanguage(), pr, sort);

                return OkOrNoContent<dynamic>(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpDelete]
        [Route("Delete")]
        [Authorize(Roles = "Admin")]
     
        public async Task<HttpResponseMessage> RemoveGood(int id)
        {
            try
            {
                await _goodService.Delete(id);

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                _log.LogWriteError("delete goods handler", e);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    string.Format("The good by id {0} was not deleted", id));
            }
        }
        [HttpPost]
        [ValidateModelMvc]
        [Route("Create")]
        public HttpResponseMessage Create(GoodsFileModel goodsApi)
        {
            Good good = null;
            try
            {
                //good = _goods.Add(goodApi.GetGood());
                Parallel.ForEach(goodsApi.Files, async f =>
                {
                    //await _photo.Add(good.GoodId, new MemoryStream(f.Data), f.FileName, f.MimeType);
                });

                //_unit.Save();
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.Accepted,
                string.Format("The good by id {0} was added successfuly", good.GoodId));
        }

        [HttpPut]
        [ValidateModelHttp]
        [Route("Update")]
        [Authorize(Roles = "Admin,Manager")]

        public async Task<HttpResponseMessage> Update(GoodsViewModel good)
        {
            try
            {
                await _goodService.UpdateOnlyFieldAsync(Mapper.Map<Good>(good),
                    g => g.GoodNameEn, g => g.GoodNameRu, g => g.PriceUsd, g => g.Discount, g => g.CategoryId);

                return Request.CreateResponse(HttpStatusCode.Accepted);
            }
            catch (Exception e)
            {

                _log.LogWriteError("Update goods exception", e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("The good by id {0} not update", good.GoodId));
            }
        }

        #region Helper methods

        [NonAction]
        private string GetCurrentCurrency()
        {
            return _storage.GetValueStorage(Request.Headers, ValuesApp.Currency)
                   ?? ValuesApp.CurrencyDefault;
        }
        [NonAction]
        private string GetCurrentLanguage()
        {
            return _storage.GetValueStorage(Request.Headers, ValuesApp.Language) ?? ValuesApp.LanguageDefault;
        }
        [NonAction]
        private IHttpActionResult OkOrNoContent<TResult>(TResult data)
        {
            if (data == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(data);
        }

        #endregion

    }
}
