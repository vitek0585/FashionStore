using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FashionStore.Application.Bootstrapper.InversionOfControl;
using FashionStore.Models.BreadCrumbs;
using FashionStore.WorkFlow.BreadCrumbs.Tag.Interfaces;
using WebLogger.Abstract.Interface;


namespace FashionStore.WorkFlow.BreadCrumbs.Container
{
    public class BreadCrumbsContainer
    {
        private ICollection<IBreadCrumbsModel> _elements;
        private ILogWriter<string> _log;
        private ITagCreator _creator;
        public BreadCrumbsContainer()
        {
            _creator = IoC.Resolve<ITagCreator>();
            _log = IoC.Resolve<ILogWriter<string>>();
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
                    if (i == _elements.Count - 1)
                    {
                        ol.InnerHtml += _creator.CreateEndTag(_elements.ElementAt(i).NameLink);
                    }
                    else
                        ol.InnerHtml += _creator.CreateTag(_elements.ElementAt(i).NameLink, _elements.ElementAt(i).Href);
                }
            }
            catch (Exception e)
            {
                _log.LogWriteError("bread crumbs construct error", e);
                return MvcHtmlString.Create(String.Empty);
            }
            return MvcHtmlString.Create(ol.ToString());
        }

    }


}