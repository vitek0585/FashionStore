using System.Web.Mvc;
using System.Web.Routing;

namespace FashionStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("*.html|js|css|gif|jpg|jpeg|png|swf");
            routes.MapMvcAttributeRoutes();
  
            
        }
    }

    
}
