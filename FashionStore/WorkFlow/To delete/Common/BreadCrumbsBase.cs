using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FashionStore.Models.BreadCrumbs;

namespace FashionStore.WorkFlow.BreadCrumbs.Common
{
    public abstract class BreadCrumbsBase
    {
        public abstract IEnumerable<IBreadCrumbsModel> GenerateBreadCrumbs(Uri url, string lang,
            params string[] links);

        
        #region Additional Methods


        protected PropertyInfo GetPropertyName<TItem>(string lang, Expression<Func<TItem, string>> expression)
            where TItem : class
        {
            var name = GetNameLamdaBody(lang, expression);
            var property = typeof(TItem).GetProperty(name,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            return property;
        }
        private string GetNameLamdaBody<TItem>(string lang, Expression<Func<TItem, string>> expression)
            where TItem : class
        {
            var exp = (MemberExpression)expression.Body;
            var str = exp.Member.Name;
            return string.Concat(str.Substring(0, str.Length - 2), lang);
        }
        #endregion

    }
}