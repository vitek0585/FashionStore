using FashionStore.Core.Common;

namespace FashionStore.Core.Bundles.User
{
    public class UserRoomBundleCss :BaseBundleCss
    {
        public UserRoomBundleCss(string path) : base(path)
        {
            Include(CssSite + "item-prod.css");
            Include(CssSite + "widget-random.css");
        }
    }
    public class UserRoomBundleJs : BaseBundleJs
    {
        public UserRoomBundleJs(string path)
            : base(path)
        {
            var pathAng = JsCore + "angular/";
            var pathTo = JsApp + "view/user/";

            Include(JsApp + "filters/filter-goods.js");
            IncludeDirectory(JsCore + "ui-bootstrap/", "*.js");

            Include(pathTo + "service/lazy-load-svc.js");
            Include(pathTo + "controller/user-room-ctrl.js");

        }
    }
}