using System.Web.Mvc;
using FashionStore.Application.Bootstrapper.InversionOfControl;
using FashionStore.Infrastructure.Data.Context.Store.Context;

namespace FashionStore.Controllers.Controller
{
    [RoutePrefix("Test")]
    public class TestController:System.Web.Mvc.Controller
    {
        private ShopContext _c;

        public TestController(ShopContext c)
        {
            _c = c;
        }

        [Route("")]
        public string Test()
        {
            return _c.GetHashCode().ToString();
        }
       
    }
}