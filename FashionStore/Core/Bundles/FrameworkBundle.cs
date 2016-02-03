using System.Web.Optimization;

namespace FashionStore.Core.Bundles
{
    public class FrameworkBundle : Bundle
    {
        public FrameworkBundle(string path):base(path,new JsMinify())
        {

            IncludeDirectory("~/Scripts/framework/cross-domain/", "*.js");
            Include("~/Scripts/framework/jquery/jquery-{version}.js");
            Include("~/Scripts/framework/jquery/modernizr-*");
            IncludeDirectory("~/Scripts/framework/bootstrap/", "*.js");
            IncludeDirectory("~/Scripts/framework/angular/", "*.js", true);
        }
    }
}