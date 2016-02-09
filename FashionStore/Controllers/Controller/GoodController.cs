using System.Web.Mvc;
using FashionStore.Controllers.Base;
using FashionStore.Core.AppValue;
using FashionStore.Models.Interfaces;
using FashionStore.Service.Interfaces.Services;
using FashionStore.WorkFlow.ViewedStorage;
using FashionStore.WorkFlow.ViewedStorage.Provider.Interface;
using WebCookie.Interfaces;

namespace FashionStore.Controllers.Controller
{
    [RoutePrefix("Fashion")]
  
    public class GoodController : ShopBaseController
    {
        private IGoodService _goodService;
        private IRecentlyViewedProvider _viewedProvider;
        private short _sizeStorageViewed;

        public GoodController(IGoodService goodService, ICookieConsumer storage, IRecentlyViewedProvider viewedProvider)
            : base(storage)
        {
            _viewedProvider = viewedProvider;
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
            var viewed = _viewedProvider.TryGet(ValuesApp.RecentlyViewed, _sizeStorageViewed);
            if (id.HasValue)
                viewed.Add(id.Value);
            return viewed;
        }

        #endregion
    }
}