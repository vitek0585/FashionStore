using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Newtonsoft.Json;

namespace FashionStore.Core.Filter.ModelValidate
{
    public class ValidateModelHttpAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                
                var json = actionContext.ModelState.SelectMany(s => s.Value.Errors, (m, e) => e.ErrorMessage);
                var msg = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest,
                    json, actionContext.ControllerContext.Configuration.Formatters.JsonFormatter);
                
                actionContext.Response = msg;

            }
        }
    }
}