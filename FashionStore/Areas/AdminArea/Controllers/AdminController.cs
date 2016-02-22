using System.Linq;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using System.Web.Mvc;
using FashionStore.Application.Services.Interfaces;
using FashionStore.Areas.AdminArea.Models;
using FashionStore.Controllers.Base;
using FashionStore.Infrastructure.Data.Identity.Interfaces.Service;
using WebCookie.Interfaces;

namespace FashionStore.Areas.AdminArea.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    [RouteArea("AdminArea", AreaPrefix = "Admin")]
    public class AdminController : ShopBaseController
    {
        private IAdminAppService _admin;
        private IRoleService _role;
        public AdminController(ICookieConsumer storage, IAdminAppService admin, IRoleService role)
            : base(storage)
        {
            _admin = admin;
            _role = role;
        }

        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("Goods")]
        public async Task<PartialViewResult> Goods()
        {
            var model = new ContainerGoodsViewModel
            {
                Category = await _admin.AllCategoryByTypeAsync<dynamic>(),
                Colors = await _admin.AllColorsAsync<dynamic>(),
                Sizes = await _admin.AllSizesAsync<dynamic>()
            };

            //await Task.WhenAll(new Task[] { categ, colors, sizes });
            //var model = new GoodsContainer()
            //{
            //    Category = categ.Result,
            //    Colors = colors.Result,
            //    Sizes = sizes.Result
            //};

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
        [Route("Users")]
        public PartialViewResult Users()
        {
            var roles = _role.All();
            return PartialView("Partial/_Users", roles);
        }
    }
}