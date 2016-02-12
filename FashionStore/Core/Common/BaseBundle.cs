using System.Web.Optimization;

namespace FashionStore.Core.Common
{
    public abstract class BaseBundleCss : Bundle
    {
        private  string _cssCommon = "~/Content/Common/";
        protected string CssCommon
        {
            get { return _cssCommon; }
         
        }

        private string _cssSite = "~/Content/Site/";
        protected string CssSite
        {
            get { return _cssSite; }
            
        }

        private string _less = "~/Content/Less/";
        protected string CssLess
        {
            get { return _less; }

        }
        protected BaseBundleCss(string path):base(path,new CssMinify())
        {
            
        }
    }
    public abstract class BaseBundleJs : Bundle
    {
        private string _jsApp = "~/Scripts/application/";

        protected string JsApp
        {
            get { return _jsApp; }
            
        }

        private string _jsCore = "~/Scripts/core/";

        protected string JsCore
        {
            get { return _jsCore; }
           
        }

        protected BaseBundleJs(string path):base(path,new JsMinify())
        {
            
        }
    }
}