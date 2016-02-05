using FashionStore.Core.Common;

namespace FashionStore.Core.Bundles.Main
{
    public class MainBundleCss : BaseBundleCss
    {
        public MainBundleCss(string path)
            : base(path)
        {
            Include(CssSite + "item-prod.css");
        }
    }

    public class MainBundleJs : BaseBundleJs
    {

        public MainBundleJs(string path)
            : base(path)
        {
            var _pathToSource = JsApp + "view/main/";
            Include(_pathToSource + "index/controller/index-ctrl.js");
        }
    }
}