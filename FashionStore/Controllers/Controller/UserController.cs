using System.Threading.Tasks;
using System.Web.Mvc;
using FashionStore.Controllers.Base;
using FashionStore.Infrastructure.Data.Identity.Interfaces.Service;
using WebCookie.Interfaces;

namespace FashionStore.Controllers.Controller
{
    [RoutePrefix("User")]
    [Authorize]
    public class UserController : AccountBaseController
    {
        private IUserAppService _userAppService;
        private int _totalPerPage;
        public UserController(ICookieConsumer storage, IUserAppService userAppService)
            : base(storage)
        {
            _userAppService = userAppService;
            _totalPerPage = 10;
        }
        [Route("Cabinet")]
        public async Task<ActionResult> Room()
        {
            var model = await _userAppService.GetUserInfoAsync(null);
            return View(model);
        }
        [Route("History")]
        public JsonResult HistoryOrder(int page)
        {
            var model = _userAppService.UserSales<dynamic>(null, page, _totalPerPage, GetCurrentCurrency(), GetCurrentLanguage());
            return JsonResultCustom(model);
        }
    }
}