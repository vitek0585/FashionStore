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
        //        <script src="~/Scripts/framework/angular/ui-router/angular-ui-router.js"></script>
        //<script src="~/Scripts/core/ui-bootstrap/ui-bootstrap-tpls-1.0.0.js"></script>

        //      <script src="~/Scripts/application/app-module/extension.js"></script>1
        //<script src="~/Scripts/application/view/admin/service/admin-crud-svc.js"></script>1
        //<script src="~/Scripts/core/angular/pagging/pagging.js"></script>1
        //<script src="~/Scripts/application/services/loading-svc.js"></script>1

        //<script src="~/Scripts/application/app-module/admin-app.js"></script>1

        //<script src="~/Scripts/application/view/admin/route/admin-route.js"></script>
        //<script src="~/Scripts/application/view/admin/controller/goods-ctrl.js"></script>
        //<script src="~/Scripts/application/view/admin/controller/edit-ctrl.js"></script>
        //<script src="~/Scripts/application/view/admin/controller/upload-ctrl.js"></script>
        //<script src="~/Scripts/application/view/admin/directive/uploadDirective.js"></script>
        //<script src="~/Scripts/application/view/admin/controller/users-ctrl.js"></script>

        //<script src="~/Scripts/application/view/admin/controller/log-ctrl.js"></script>
        public AdminBundleJs(string path) : base(path)
        {
            var pathAct = JsApp + "view/admin/";
            var angCore = JsCore + "angular/";

            Include("~/Scripts/framework/angular/ui-router/angular-ui-router.js");
            Include("~/Scripts/core/ui-bootstrap/ui-bootstrap-tpls-1.0.0.js");

            IncludeDirectory(angCore + "combo-box-selected-item/", "*.js");

            //Include(JsApp + "app-module/extension.js");
            //Include(pathAct + "service/admin-crud-svc.js");
            //Include(angCore + "pagging/pagging.js");
            //Include(JsApp + "services/loading-svc.js");
            //Include(JsApp + "app-module/admin-app.js");
            //Include(pathAct + "route/admin-route.js");
            //Include(pathAct + "controller/goods-ctrl.js");
            //Include(pathAct + "controller/edit-ctrl.js");
            //Include(pathAct + "controller/upload-ctrl.js");
            //Include(pathAct + "directive/uploadDirective.js");
            //Include(pathAct + "controller/users-ctrl.js");
            //Include(pathAct + "controller/log-ctrl.js");






        }
    }
}