using System.Web.Mvc;
using FashionStore.Controllers.Base;
using FashionStore.Core.AppValue;
using FashionStore.Models.Interfaces;
using FashionStore.Service.Interfaces.Services;
using WebCookie.Interfaces;

namespace FashionStore.Controllers.Controller
{

    [RoutePrefix("Main")]
    public class MainController : ShopBaseController
    {
        private IGoodService _goodService;
        private ICategoryService _categoryService;


        public MainController(IGoodService goodService, ICategoryService category, ICookieConsumer storage)
            : base(storage)
        {
            _categoryService = category;
            _goodService = goodService;

        }
        [Route("~/")]
        public ActionResult Index()
        {
            var randCount = 8;
            ViewBag.RandCategories = _categoryService.GetRandom<ICategoryModel>(randCount, GetCurrentLanguage());

            return View();
        }

        [Route("ChangeLanguage")]
        public ActionResult ChangeLanguage(string lang, string refUrl)
        {
            _storage.SetValueStorage(ControllerContext.HttpContext, ValuesApp.Language,
                lang, ValuesApp.Languages);

            return Redirect(CheckValidReturnUrl(refUrl));
        }
        [Route("ChangeCurrency")]
        public ActionResult ChangeCurrency(string currency, string refUrl)
        {
            _storage.SetValueStorage(ControllerContext.HttpContext, ValuesApp.Currency,
                currency, ValuesApp.Currencies);

            return Redirect(CheckValidReturnUrl(refUrl));
        }
        #region Child Action
        [ChildActionOnly]
        [Route("NavBarMenu")]
        public PartialViewResult NavBarMenu()
        {
            var data = _categoryService.AllCategory<ITypeCategoryModel<ICategoryModelBase>>(GetCurrentLanguage());
            return PartialView(data);
        }

        #endregion

    }
}