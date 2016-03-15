using FashionStore.Core.Common;

namespace FashionStore.Core.Bundles.Account
{
    public class AccountBundleJs:BaseBundleJs
    {
        public AccountBundleJs(string path) : base(path)
        {
            var pathTo = JsApp + "view/account/";
            Include(pathTo + "controller/login-confirm-ctrl.js");
        }
    }
}