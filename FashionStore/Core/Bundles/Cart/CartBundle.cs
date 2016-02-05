using FashionStore.Core.Common;

namespace FashionStore.Core.Bundles.Cart
{
    public class CartBundleCss : BaseBundleCss
    {
        public CartBundleCss(string path)
            : base(path)
        {
            IncludeDirectory(CssCommon + "combo-box-bootstrap/", "*.css");
            Include(CssSite + "select-picker.css");
            Include(CssSite + "detail-item.css");
            Include(CssSite + "widget-random.css");
            Include(CssSite + "item-prod.css");
        }
    }

    public class CartBundleJs : BaseBundleJs
    {
        public CartBundleJs(string path)
            : base(path)
        {
            var pathAng = JsCore + "angular/";
            var pathTo = JsApp + "view/cart/";

            IncludeDirectory(pathAng + "combo-box-bootstrap/", "*.js");
            IncludeDirectory(pathAng + "filters/", "*.js");
            Include(pathTo + "directive/select-picker-dir.js");
            Include(JsApp + "filters/filter-goods.js");
            Include(JsApp + "directives/dialog-window.js");


            Include(pathTo + "controller/cart-ctrl.js");
            Include(pathTo + "controller/fast-check-ctrl.js");
            Include(pathTo + "controller/goods-details-ctrl.js");


        }
    }

}