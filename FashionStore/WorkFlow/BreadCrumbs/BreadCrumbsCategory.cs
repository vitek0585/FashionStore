using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Models.BreadCrumbs;
using FashionStore.WorkFlow.BreadCrumbs.Common;

namespace FashionStore.WorkFlow.BreadCrumbs
{
    public class BreadCrumbsCategory : BreadCrumbsBase
    {
        private ICategoryRepository _categoryRepository;
        private UrlHelper _urlHelper;
        private int _categoryId;
        public BreadCrumbsCategory(int id, UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
            _categoryId = id;
            _categoryRepository = DependencyResolver.Current.GetService<ICategoryRepository>();
        }

        protected IEnumerable<IBreadCrumbsCategoryModel> CategoryWithChild(int id, string lang, params string[] links)
        {
            var data = _categoryRepository.EnableProxy<ICategoryRepository>().GetAll()
                .Where(c => c.CategoryId == id).Include(c => c.Parent).Include(c => c.Parent.Parent)
                .Include(c => c.Parent.Parent.Parent).Include(c => c.Name)
                .Include(c => c.Type)
                .FirstOrDefault();

            if (data == null)
                throw new ArgumentException(string.Format("Category by id {0} not found", id));

            var result = CategoryAndChild(data).Select(c => new
            {
                c.CategoryId,
                NameLink =
                    GetPropertyName<CategoryName>(lang, cn => cn.CategoryNameRu).GetValue(c.Name),
                TypeName = GetPropertyName<CategoryType>(lang, cn => cn.TypeNameRu).GetValue(c.Type),
                TypeHref = c.Type.TypeNameEn
            });

            var linksMutable = links
                .Concat(Enumerable.Repeat(result.First().TypeName.ToString(), 1))
                .Select(l => new { NameLink = l, CategoryId = 0, TypeName = result.First().TypeName, TypeHref = result.First().TypeHref })
                .Concat<dynamic>(result.Reverse()).Select(l => new { l.NameLink, l.CategoryId, l.TypeName,l.TypeHref }).AsEnumerable();

            return AutoMapper.Mapper.DynamicMap<IEnumerable<IBreadCrumbsCategoryModel>>(linksMutable);

        }

        private IEnumerable<Category> CategoryAndChild(Category data)
        {
            var source = data;
            do
            {
                yield return source;
                source = source.Parent;

            } while (source != null);
        }

        public override IEnumerable<IBreadCrumbsModel> GenerateBreadCrumbs(Uri url, string lang, params string[] links)
        {
            var newLinks = CategoryWithChild(_categoryId, lang, links);
            return CreateLinks(newLinks.ToArray()); 
        }
        protected IEnumerable<IBreadCrumbsModel> CreateLinks(IBreadCrumbsCategoryModel[] links)
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
                    Href = _urlHelper.Action("Category", "Catalog", new { type = model.TypeHref, id = model.CategoryId })
                };
            }
            
        }
       

    }
}