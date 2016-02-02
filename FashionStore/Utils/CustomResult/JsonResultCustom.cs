using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FashionStore.Utils.CustomResult
{
    public class JsonResultCustom : JsonResult
    {
        private HttpStatusCode _status;

        public JsonResultCustom(object data, HttpStatusCode status)
        {
            _status = status;
            Data = JsonConvert.SerializeObject(data,new JsonSerializerSettings()
            {
                DateFormatString = "dd/MM/yyyy",
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
        
        public JsonResultCustom(string data, HttpStatusCode status)
        {
            _status = status;
            Data = data;
        }
        
        public JsonResultCustom(object data)
            : this(data, HttpStatusCode.OK)
        {

        }
        public override void ExecuteResult(ControllerContext context)
        {
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)_status;
            response.Write(Data);
        }

    }
}