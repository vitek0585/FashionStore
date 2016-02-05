using FashionStore.Core.Common;

namespace FashionStore.Core.Bundles.Catalog
{
    public class CatalogCategoriesBundleCss : BaseBundleCss
    {

        public CatalogCategoriesBundleCss(string path)
            : base(path)
        {
            Include(CssSite + "categories.css");

        }
    }
    public class CatalogCategoryBundleCss : BaseBundleCss
    {

        public CatalogCategoryBundleCss(string path)
            : base(path)
        {
            Include(CssSite + "item-prod.css");
            Include(CssSite + "widget-random.css");
            IncludeDirectory(CssCommon + "range-slider/","*.css");
            IncludeDirectory(CssCommon + "combo-box-select-item/", "*.css");
            Include(CssSite + "combo-box-filter.css");

        }
    }
    public class CatalogCategoryBundleJs : BaseBundleJs
    {

        public CatalogCategoryBundleJs(string path)
            : base(path)
        {
            var pathAct = JsApp + "view/catalog/";
            var angCore = JsCore + "angular/";

            IncludeDirectory(angCore + "range-slider/", "*.js");
            IncludeDirectory(angCore + "combo-box-selected-item/", "*.js");
            IncludeDirectory(JsCore + "ui-bootstrap/", "*.js");
            IncludeDirectory(angCore + "pagging/", "*.js");

            Include(pathAct + "controller/category-ctrl.js");

        }
    }
}