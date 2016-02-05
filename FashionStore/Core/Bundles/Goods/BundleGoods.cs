using FashionStore.Core.Common;

namespace FashionStore.Core.Bundles.Goods
{
    public class GoodsDetailBundleCss:BaseBundleCss
    {
        public GoodsDetailBundleCss(string path) : base(path)
        {
            IncludeDirectory(CssCommon + "combo-box-bootstrap/", "*.css");
            IncludeDirectory(CssCommon + "zoom-img/", "*.css");
            Include(CssSite + "select-picker.css");
            Include(CssSite + "detail-item.css");
            Include(CssSite + "widget-random.css");
        }
    }
    public class GoodsDetailBundleJs : BaseBundleJs
    {
        public GoodsDetailBundleJs(string path)
            : base(path)
        {
            var pathAng = JsCore + "angular/";
            var pathTo = JsApp + "view/goods/";

            IncludeDirectory(pathAng + "combo-box-bootstrap/", "*.js");
            IncludeDirectory(pathAng + "filters/", "*.js");
            IncludeDirectory(pathAng + "image-zoom/", "*.js");

            Include(JsApp + "filters/filter-goods.js");
            Include(pathTo + "controller/detail-ctrl.js");
        }
    }
}