using System.Web.Optimization;
using FashionStore.Core.Bundles;
using FashionStore.Core.Bundles.Catalog;
using FashionStore.Core.Bundles.Layout;
using FashionStore.Core.Bundles.Main;

namespace FashionStore
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new FrameworkBundle("~/bundles/base"));

            bundles.Add(new LayuotBundleCss("~/bundles/layoutCss"));
            bundles.Add(new LayuotBundleJs("~/bundles/layoutJs"));

            bundles.Add(new MainBundleCss("~/bundles/mainCss"));
            bundles.Add(new MainBundleJs("~/bundles/mainJs"));

            bundles.Add(new CatalogCategoriesBundleCss("~/bundles/categoriesCss"));

            bundles.Add(new CatalogCategoryBundleCss("~/bundles/categoryCss"));
            bundles.Add(new CatalogCategoryBundleJs("~/bundles/categoryJs"));


        }
    }
}
