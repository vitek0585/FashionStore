using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Web.Mvc;
using FashionStore.Models.BreadCrumbs;
using FashionStore.WorkFlow.BreadCrumbs.Container;
using FashionStore.WorkFlow.BreadCrumbs.Container.Base;

namespace FashionStore.WorkFlow.BreadCrumbs.Builders.Base
{
    public abstract class BreadCrumbsBuilder
    {
        protected BreadCrumbsContainerBase Container { get; set; }

        protected string MainName { get; set; }

        protected string CurrentSelectionName { get; set; }

        protected UrlHelper UrlHelper { get; set; }

        #region constructor

        protected BreadCrumbsBuilder(UrlHelper urlHelper, string mainName = null, string currentSelectionName = null)
        {
            Container = new BreadCrumbsContainer();
            CurrentSelectionName = currentSelectionName;
            MainName = mainName;
            UrlHelper = urlHelper;
        }


        #endregion

        public virtual void MainBuild()
        {
            var main = new BreadCrumbsModel()
                {
                    NameLink = MainName,
                    Href = UrlHelper.Action("Index", "Main")
                };
            Container.AddElement(main);
        }

        public abstract void PathBuild();

        public virtual void CurrentSelectionBuild()
        {
            var currentSelection = new BreadCrumbsModel()
            {
                NameLink = CurrentSelectionName
            };
            Container.AddElement(currentSelection);
        }

        public MvcHtmlString GetResult()
        {
            return Container.Generate();
        }

        #region helper methods
        protected string GetCurrentLanguage()
        {
            return Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        }
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
