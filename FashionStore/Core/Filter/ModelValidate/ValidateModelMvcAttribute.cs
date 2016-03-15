using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace FashionStore.Core.Filter.ModelValidate
{

    public class ValidateModelMvcAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                var response = filterContext.RequestContext.HttpContext.Response;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                //var errors = filterContext.Controller.ViewData.ModelState;
                filterContext.Result = new JsonResult()
                {
                    ContentType = "application/json",
                    Data = filterContext.Controller.ViewData.ModelState.SelectMany(s => s.Value.Errors, (m, e) => e.ErrorMessage),
                    //Data = errors.Keys.SelectMany(key => errors[key].Errors,
                    //(k, e) => new { key = k, error = e.ErrorMessage })

                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}