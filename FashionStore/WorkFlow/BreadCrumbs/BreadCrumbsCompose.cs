using System.Web.Mvc;
using FashionStore.WorkFlow.BreadCrumbs.Builders.Base;

namespace FashionStore.WorkFlow.BreadCrumbs
{
    public class BreadCrumbsCompose
    {
        private BreadCrumbsBuilder _builderCrumbs;

        public BreadCrumbsCompose(BreadCrumbsBuilder builderCrumbs)
        {
            _builderCrumbs = builderCrumbs;
        }

        public MvcHtmlString Construct()
        {
            _builderCrumbs.MainBuild();
            _builderCrumbs.PathBuild();
            _builderCrumbs.CurrentSelectionBuild();
            return _builderCrumbs.GetResult();
        }
    }
}