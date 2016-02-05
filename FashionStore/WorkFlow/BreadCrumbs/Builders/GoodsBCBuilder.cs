using System.Linq;
using System.Web.Mvc;
using FashionStore.Models.BreadCrumbs;

namespace FashionStore.WorkFlow.BreadCrumbs.Builders
{
    public class GoodsBcBuilder:CategoryBcBuilder
    {
        public GoodsBcBuilder(UrlHelper urlHelper, int categoryId, string mainName = null, string currentSelectionName = null)
            : base(urlHelper,categoryId, mainName, currentSelectionName)
        {
        }
        public override void PathBuild()
        {
            var links = CategoryWithChild(CategoryId);
            CurrentCatalog(links.First());
            //skip the last element so he is our link
            CategoryTree(links);
        }

        public override void CurrentSelectionBuild()
        {
            var item = new BreadCrumbsModel()
            {
                NameLink = CurrentSelectionName
            };
            Container.AddElement(item);
        }
    }
}