using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FashionStore.Models.BreadCrumbs;

namespace FashionStore.WorkFlow.BreadCrumbs
{
    public class BreadCrumbsGood : BreadCrumbsCategory
    {
        private int _categoryId;
        private UrlHelper _urlHelper;
        private string _goodName { get; set; }

        public BreadCrumbsGood(int categoryId, string goodName,UrlHelper urlHelper)
            : base(categoryId,urlHelper)
        {
            _goodName = goodName;
            _urlHelper = urlHelper;
            _categoryId = categoryId;
        }
        public override IEnumerable<IBreadCrumbsModel> GenerateBreadCrumbs(Uri url, string lang, params string[] links)
        {
            var r = CategoryWithChild(_categoryId, lang, links);
            return CreateLinks(r.ToArray());
        }
        protected new IEnumerable<IBreadCrumbsModel> CreateLinks(IBreadCrumbsCategoryModel[] links)
        {
            int i = 0;
            yield return new BreadCrumbsModel()
            {
                NameLink = links[i].NameLink,
                Href = _urlHelper.Action("Categories", "Catalog", new { type = links[i].TypeHref })
            };
            i++;
            foreach (var model in links.Skip(i))
            {
                yield return new BreadCrumbsModel()
                {
                    NameLink = model.NameLink,
                    Href = _urlHelper.Action("Category", "Catalog",new {type = model.TypeHref,id = model.CategoryId})
                };
            }
            yield return new BreadCrumbsModel()
            {
                NameLink = _goodName
            };
        }
        
    }
}