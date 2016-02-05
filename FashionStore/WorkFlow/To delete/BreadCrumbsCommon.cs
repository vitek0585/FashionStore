using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FashionStore.Models.BreadCrumbs;
using FashionStore.WorkFlow.BreadCrumbs.Common;

namespace FashionStore.WorkFlow.BreadCrumbs
{

    public class BreadCrumbsCommon : BreadCrumbsBase
    {
        private UrlHelper _urlHelper;

        public BreadCrumbsCommon(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public override IEnumerable<IBreadCrumbsModel> GenerateBreadCrumbs(Uri url, string lang, params string[] links)
        {
            
            yield return new BreadCrumbsModel()
            {
                Href = _urlHelper.Action("Categories","Catalog",new {type = url.Segments[1]}),
                NameLink = links[0]
            };
        }

        
    }
}