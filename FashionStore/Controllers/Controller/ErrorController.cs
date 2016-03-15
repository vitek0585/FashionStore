using System.Web.Mvc;

namespace FashionStore.Controllers.Controller
{
    [RoutePrefix("Error")]
    public class ErrorController : System.Web.Mvc.Controller
    {
        [Route("NotFound")]
        public ActionResult NotFound()
        {
            return View();
        }
        [Route("InternalError")]
        public ActionResult ServerError()
        {
            return View();
        }
       
    }
}