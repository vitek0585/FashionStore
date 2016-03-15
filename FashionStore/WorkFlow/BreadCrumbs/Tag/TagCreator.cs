using System.Web.Mvc;
using FashionStore.WorkFlow.BreadCrumbs.Tag.Interfaces;

namespace FashionStore.WorkFlow.BreadCrumbs.Tag
{
    public class TagCreator : ITagCreator
    {
        public string CreateTag(string textHtml, string href)
        {
            var li = new TagBuilder("li");
            TagBuilder a = new TagBuilder("a");
            a.MergeAttribute("href", href);
            a.SetInnerText(textHtml.ToUpper());
            li.InnerHtml += a.ToString();
            return li.ToString();
        }

        public string CreateEndTag(string textHtml)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("active");
            var span = new TagBuilder("span");
            span.SetInnerText(textHtml.ToUpper());
            li.InnerHtml += span.ToString();
            return li.ToString();
        }
    }
}