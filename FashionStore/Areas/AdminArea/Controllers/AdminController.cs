using System.Threading.Tasks;
using System.Web.Mvc;
using FashionStore.Application.Services.Interfaces;
using FashionStore.Controllers.Base;
using WebCookie.Interfaces;

namespace FashionStore.Areas.AdminArea.Controllers
{
    [RouteArea("AdminArea", AreaPrefix = "Admin")]
    public class AdminController : ShopBaseController
    {
        private IAdminAppService _admin;
        public AdminController(ICookieConsumer storage, IAdminAppService admin)
            : base(storage)
        {
            _admin = admin;
        }
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("Goods")]
        public async Task<PartialViewResult> Goods()
        {
            var model = await _admin.AllCategoryByTypeAsync<dynamic>();
            return PartialView("Partial/_Goods", model);
        }
        [Route("GoodsTable")]
        public PartialViewResult GoodsTable()
        {
            return PartialView("Partial/_GoodsTable");
        }
        [Route("GoodsEdit")]
        public PartialViewResult GoodsEdit()
        {
            return PartialView("Partial/_GoodsEdit");
        }
        [Route("Log")]
        public PartialViewResult Log()
        {
            return PartialView("Partial/_Log");
        }
    }
}