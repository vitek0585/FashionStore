using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using FashionStore.WorkFlow.BreadCrumbs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FashionStore.Utils.Helpers
{
    public static class HtmlExtensionsBreadCrumbs
    {
        public static MvcHtmlString GenerateBreadCrumbs(this HtmlHelper html, BreadCrumbsCompose compose)
        {
            return compose.Construct();
        }

        private static string CreateTagLink(string text, string href)
        {
            //TagBuilder ol = new TagBuilder("ol");
            //ol.AddCssClass("breadcrumb");

            //try
            //{
            //    var lang = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            //    Uri url = html.ViewContext.HttpContext.Request.Url;
            //    var data = crumbs.GenerateBreadCrumbs(url, lang, links);

            //    TagBuilder li = new TagBuilder("li");
            //    li.InnerHtml += CreateTagLink(Resource.Main, new UriBuilder(url.Scheme, url.Host, url.Port).ToString());
            //    ol.InnerHtml += li.ToString();
            //    for (var i = 0; i < data.Count(); i++)
            //    {

            //        li = new TagBuilder("li");
            //        if (i == data.Count() - 1)
            //        {
            //            li.AddCssClass("active");
            //            TagBuilder tb = new TagBuilder("span");
            //            tb.SetInnerText(data.ElementAt(i).NameLink);
            //            li.InnerHtml += tb.ToString();
            //        }
            //        else
            //            li.InnerHtml += CreateTagLink(data.ElementAt(i).NameLink, data.ElementAt(i).Href);

            //        ol.InnerHtml += li.ToString();
            //    }
            //}
            //catch (Exception e)
            //{
            //    return MvcHtmlString.Create(e.Message + String.Empty);
            //}

            TagBuilder tb = new TagBuilder("a");
            tb.MergeAttribute("href", href);
            tb.SetInnerText(text.ToUpper());
            return tb.ToString();
        }
    }

    public static class HtmlExtensionsMapToJson
    {
        public static string SerializeToJson<TItem>(this HtmlHelper html, TItem item)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(item, settings);
        }

    }
    public static class ExpressionToString
    {
        public static string GetName<T>(this HtmlHelper<T> html, Expression<Func<T, object>> exp)
        {
            return ExpressionHelper.GetExpressionText(exp);
        }
        public static string GetName<T>(this HtmlHelper html, Expression<Func<T, object>> exp, int? endRemove = null)
        {
            if (exp.Body.NodeType == ExpressionType.Convert || exp.Body.NodeType == ExpressionType.ConvertChecked)
            {
                var uexp = (UnaryExpression)exp.Body;
                return FirstCharacterToLow(((MemberExpression)uexp.Operand).Member.Name);
            }
            var str = FirstCharacterToLow(ExpressionHelper.GetExpressionText(exp));
            if (endRemove.HasValue)
                return str.Substring(0, str.Length - endRemove.Value);

            return str;
        }

        private static string FirstCharacterToLow(string name)
        {
            return Char.ToLowerInvariant(name[0]) + name.Substring(1);
        }

    }
}