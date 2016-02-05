using System.Web.Mvc;
using FashionStore.Controllers.Base;
using FashionStore.Core.AppValue;
using FashionStore.Models.Interfaces;
using FashionStore.Service.Interfaces.Services;
using FashionStore.WorkFlow.ViewedStorage;
using WebCookie.Interfaces;

namespace FashionStore.Controllers.Controller
{
    [RoutePrefix("Fashion")]
  
    public class GoodController : ShopBaseController
    {
        private IGoodService _goodService;
        private short _sizeStorageViewed;

        public GoodController(IGoodService goodService,ICookieConsumer storage)
            : base(storage)
        {
            _goodService = goodService;
            _sizeStorageViewed = 20;
        }
        [Route("Good/{id:min(1):max(10000000)}")]
        public ActionResult GetDetails(int id)
        {
            RecentlyViewed(id);
            var data = _goodService.GetGood<IGoodModel>(id, GetCurrentCurrency(), GetCurrentLanguage());
            
            return View("Details",data);
        }

        #region Child action

        [ChildActionOnly, Route("RecentlyViewedUser")]
        public ActionResult RecentlyViewedUser()
        {
            var ids = RecentlyViewed(null).GetAll();
            var data = _goodService.GetGoods<IGoodModel>(ids, GetCurrentLanguage());
            return PartialView("Widgets/_RecentlyViewed",data);

        }

        #endregion
        #region Helper
        [NonAction]
        protected RecentlyViewedStorage RecentlyViewed(int? id)
        {
            var viewed = (RecentlyViewedStorage)Session[ValuesApp.RecentlyViewed];
            if (viewed == null)
            {
                Session[ValuesApp.RecentlyViewed] = viewed = new RecentlyViewedStorage(_sizeStorageViewed);
            }

            if (id.HasValue)
                viewed.Add(id.Value);

            return viewed;
        }

        #endregion
    }
}