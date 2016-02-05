using FashionStore.Core.Common;

namespace FashionStore.Core.Bundles.Layout
{
    public class LayuotBundleCss : BaseBundleCss
    {

        public LayuotBundleCss(string path)
            : base(path)
        {

            Include(CssCommon + "/bootstrap/bootstrap.css");
            Include(CssSite + "site.css");
            Include(CssSite + "nav-bar.css");

            IncludeDirectory(CssCommon + "effect-button/", "*.css");
            IncludeDirectory(CssCommon + "modal/", "*.css");
            Include(CssSite + "additional-view.css");
            IncludeDirectory(CssCommon + "notify/", "*.css");
            IncludeDirectory(CssCommon + "slick/", "*.css");
            IncludeDirectory(CssCommon + "tooltip/", "*.css");
            Include(CssSite + "slick-carousel.css");

        }

    }
    public class LayuotBundleJs : BaseBundleJs
    {

        public LayuotBundleJs(string path)
            : base(path)
        {
            var _jsLayout = JsApp + "view/layout/";
            var _jsCommon = JsCore + "angular/";

            Include(_jsCommon + "slick/slick.min.js");
            Include(_jsCommon + "slick/slick-angular.js");
            IncludeDirectory(_jsCommon + "notify/", "*.js");
            IncludeDirectory(_jsCommon + "modal-window/", "*.js");
            IncludeDirectory(_jsCommon + "convert-formdata-to-obj/", "*.js");

            Include(JsApp + "services/http-svc.js");
            IncludeDirectory(_jsCommon + "to-up-web-page/", "*.js");
            IncludeDirectory(_jsCommon + "spinner/", "*.js");
            IncludeDirectory(_jsCommon + "form-comparer/", "*.js");
            IncludeDirectory(_jsCommon + "tooltip/", "*.js");


            Include(JsApp + "services/cart-svc.js");
            Include(JsApp + "app-module/global-app.js");

            Include(_jsLayout + "controller/modal-ctrl.js");
            Include(_jsLayout + "controller/cart-state-ctrl.js");



        }

    }
}