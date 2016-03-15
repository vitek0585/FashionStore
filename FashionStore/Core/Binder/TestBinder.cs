//using System.Web.ModelBinding;


using System.Collections.Specialized;
using System.Web.Mvc;



namespace FashionStore.Core.Binder
{
    public class TestBinder:IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var str = controllerContext.RequestContext.HttpContext.Request.Form;
            var newCol = new NameValueCollection(str) {{"vitek", "1"}};
        
            return newCol;
           
        }
    }
}