using System.Web.Mvc;
using FashionStore.WorkFlow.BreadCrumbs.Builders.Base;

namespace FashionStore.WorkFlow.BreadCrumbs.Builders
{
    public class SimpleBcBuilder:BreadCrumbsBuilder
    {
        public SimpleBcBuilder(UrlHelper urlHelper, string mainName = null, string currentSelectionName = null)
            : base(urlHelper, mainName, currentSelectionName)
        {
        }

        public override void PathBuild()
        {

        }
    }
}