using System.Collections.Generic;
using System.Web.Mvc;
using FashionStore.Models.BreadCrumbs;
using FashionStore.WorkFlow.BreadCrumbs.Tag.Interfaces;
using WebLogger.Abstract.Interface;

namespace FashionStore.WorkFlow.BreadCrumbs.Container.Base
{
    public abstract class BreadCrumbsContainerBase
    {
        protected ICollection<IBreadCrumbsModel> _elements;
        protected ITagCreator _creator;


        public void AddElement(IBreadCrumbsModel element)
        {
            _elements.Add(element);
        }

        public abstract MvcHtmlString Generate();
    }
}