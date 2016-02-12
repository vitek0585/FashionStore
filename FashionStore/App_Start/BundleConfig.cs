using System.Web.Optimization;
using FashionStore.Core.Bundles;
using FashionStore.Core.Bundles.Account;
using FashionStore.Core.Bundles.Admin;
using FashionStore.Core.Bundles.Cart;
using FashionStore.Core.Bundles.Catalog;
using FashionStore.Core.Bundles.Goods;
using FashionStore.Core.Bundles.Layout;
using FashionStore.Core.Bundles.Main;
using FashionStore.Core.Bundles.User;

namespace FashionStore
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new FrameworkBundle("~/bundles/base"));

            #region layout

            bundles.Add(new LayuotBundleCss("~/bundles/layoutCss"));
            bundles.Add(new LayuotBundleJs("~/bundles/layoutJs"));

            #endregion

            #region main

            bundles.Add(new MainBundleCss("~/bundles/mainCss"));
            bundles.Add(new MainBundleJs("~/bundles/mainJs"));

            #endregion

            #region catalog

            bundles.Add(new CatalogCategoriesBundleCss("~/bundles/categoriesCss"));

            bundles.Add(new CatalogCategoryBundleCss("~/bundles/categoryCss"));
            bundles.Add(new CatalogCategoryBundleJs("~/bundles/categoryJs"));

            #endregion

            #region goods

            bundles.Add(new GoodsDetailBundleCss("~/bundles/detailCss"));
            bundles.Add(new GoodsDetailBundleJs("~/bundles/detailJs"));

            #endregion

            #region cart

            bundles.Add(new CartBundleCss("~/bundles/cartCss"));
            bundles.Add(new CartBundleJs("~/bundles/cartJs"));

            #endregion

            #region account

            bundles.Add(new AccountBundleJs("~/bundles/accountJs"));

            #endregion

            #region user

            bundles.Add(new UserRoomBundleCss ("~/bundles/roomCss"));
            bundles.Add(new UserRoomBundleJs("~/bundles/roomJs"));

            #endregion

            #region admin

            bundles.Add(new AdminBundleCss("~/bundles/adminCss"));
            bundles.Add(new AdminBundleJs("~/bundles/adminJs"));
            #endregion

        }
    }
}
