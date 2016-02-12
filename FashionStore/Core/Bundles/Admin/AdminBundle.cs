using FashionStore.Core.Common;

namespace FashionStore.Core.Bundles.Admin
{
    public class AdminBundleCss:BaseBundleCss
    {
        public AdminBundleCss(string path) : base(path)
        {
            Include(CssCommon + "/bootstrap-admin/bootstrap-admin.css");
            IncludeDirectory(CssCommon + "effect-button/", "*.css");
            IncludeDirectory(CssCommon + "modal/", "*.css");
            IncludeDirectory(CssCommon + "notify/", "*.css");
            IncludeDirectory(CssCommon + "slick/", "*.css");
            IncludeDirectory(CssCommon + "tooltip/", "*.css");
            IncludeDirectory(CssCommon + "combo-box-select-item/", "*.css");

            Include(CssLess + "Admin/Admin.css");
        }
    }
    public class AdminBundleJs:BaseBundleJs
    {
        public AdminBundleJs(string path) : base(path)
        {
            var pathAct = JsApp + "view/catalog/";
            var angCore = JsCore + "angular/";
            IncludeDirectory(angCore + "combo-box-selected-item/", "*.js");
        }
    }
}