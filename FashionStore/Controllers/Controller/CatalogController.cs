using System.Web.Mvc;
using FashionStore.Controllers.Base;
using FashionStore.Models.Interfaces;
using FashionStore.Service.Interfaces.Services;
using WebCookie.Interfaces;

namespace FashionStore.Controllers.Controller
{
    using ICatModel = ITypeCategoryModel<ICategoryModel>;

    [RoutePrefix("Catalog")]
    public class CatalogController : ShopBaseController
    {
        private ICategoryService _categoryService;

        private short _sizeStorageViewed;
        private byte _totalPerPage = 9;

        public CatalogController(ICategoryService categoryService, ICookieConsumer storage)
            : base(storage)
        {
 
            _categoryService = categoryService;
            _sizeStorageViewed = 5;
        }
        [Route("{type}")]
        public ActionResult Categories(string type)
        {
            var data = _categoryService.GetCategoriesByType<ICatModel>(type, GetCurrentLanguage());

            if (data != null)
            {
                return View(data);
            }

            return HttpNotFound();

        }
        [Route("{type}/Category/{id:int:min(1):max(100000)}")] //{page:int:min(1):max(1000)=1}")]
        public ActionResult Category(string type, int id)//, int page)
        {
            var data = _categoryService.GetCategoryByCulture<ICategoryDescModel>(type, id, GetCurrentLanguage());

            if (data != null)
            {
                //ViewBag.Page = page;
                return View(data);
            }

            return HttpNotFound();

        }


        #region Child action

        [ChildActionOnly, Route("Filter")]
        public ActionResult Filter(int id)
        {
            var data = _categoryService.GetInformationAboutCategory<IFilterModel>(id, GetCurrentCurrency(),
                GetCurrentLanguage());
            return PartialView("Partial/_Filter",data);
        }

        [ChildActionOnly, Route("Sale/{type}/{discount:min(0):max(100):int?}")]
        public ActionResult Sale(string type, int discount = 50)
        {
            var data = _categoryService.GetCategoriesSale<ICatModel>(type, GetCurrentLanguage(), discount);

            return PartialView("Partial/_Sale",data);
        }

        #endregion

        
    }
}