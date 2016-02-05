using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FashionStore.Models.BreadCrumbs;

namespace FashionStore.WorkFlow.BreadCrumbs.Container
{
    public class BreadCrumbsContainer
    {
        private ICollection<IBreadCrumbsModel> _elements;

        public BreadCrumbsContainer()
        {
            _elements = new List<IBreadCrumbsModel>();
        }

        public void AddElement(IBreadCrumbsModel element)
        {
            _elements.Add(element);
        }
       
        public MvcHtmlString Generate()
        {
            TagBuilder ol = new TagBuilder("ol");
            ol.AddCssClass("breadcrumb");

            try
            {
                for (var i = 0; i < _elements.Count; i++)
                {
                TagBuilder li = new TagBuilder("li");

                    li = new TagBuilder("li");
                    if (i == _elements.Count - 1)
                    {
                        li.AddCssClass("active");
                        TagBuilder tb = new TagBuilder("span");
                        tb.SetInnerText(_elements.ElementAt(i).NameLink);
                        li.InnerHtml += tb.ToString();
                    }
                    else
                        li.InnerHtml += CreateTagLink(_elements.ElementAt(i).NameLink, _elements.ElementAt(i).Href);

                    ol.InnerHtml += li.ToString();
                }
            }
            catch (Exception e)
            {
                return MvcHtmlString.Create(e.Message + String.Empty);
            }
            return MvcHtmlString.Create(ol.ToString());
        }

        private string CreateTagLink(string text, string href)
        {
            TagBuilder tb = new TagBuilder("a");
            tb.MergeAttribute("href", href);
            tb.SetInnerText(text.ToUpper());
            return tb.ToString();
        }
    }

        
}