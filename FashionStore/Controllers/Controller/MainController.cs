using System;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using FashionStore.Controllers.Base;
using FashionStore.Core.AppValue;
using FashionStore.Core.Binder;
using FashionStore.Models.Interfaces;
using FashionStore.Service.Interfaces.Services;
using WebCookie.Interfaces;

namespace FashionStore.Controllers.Controller
{

    [RoutePrefix("Main")]
    public class MainController : ShopBaseController
    {

        private ICategoryService _categoryService;


        public MainController(ICategoryService category, ICookieConsumer storage)
            : base(storage)
        {
            _categoryService = category;
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
        [Route("NotFound")]
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            //throw new Exception("SS");
            return View("NotFoundError");
        }
       
        [Route("Error"),HttpGet]
        public JsonResult Error()//[ModelBinder(typeof(TestBinder))]NameValueCollection col)
        {
            throw  new Exception("Error TEST");
            //return Json(col.AllKeys.ToDictionary(k=>k,k=>col.Get(k)), JsonRequestBehavior.AllowGet);
        }

        #region Child Action
        [ChildActionOnly]
        [Route("NavBar")]
        public PartialViewResult NavBar()
        {
            var data = _categoryService.AllCategory<ITypeCategoryModel<ICategoryModelBase>>(GetCurrentLanguage());
            return PartialView(data);
        }

        #endregion

    }
}