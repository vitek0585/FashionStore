using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using FashionStore.Application.Bootstrapper.InversionOfControl;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Models.BreadCrumbs;
using FashionStore.WorkFlow.BreadCrumbs.Builders.Base;

namespace FashionStore.WorkFlow.BreadCrumbs.Builders
{
    public class CategoryBcBuilder : BreadCrumbsBuilder
    {
        protected readonly int CategoryId;
        private IEnumerable<IBreadCrumbsCategoryModel> _links;
        public CategoryBcBuilder(UrlHelper urlHelper, int categoryId, string mainName = null, 
            string currentSelectionName = null)
            : base(urlHelper, mainName, currentSelectionName)
        {
            CategoryId = categoryId;

        }

        public override void PathBuild()
        {
            _links = CategoryWithChild(CategoryId);
            CurrentCatalog(_links.First());
            //skip the last element so he is our link
            CategoryTree(_links.Take(_links.Count() - 1));
        }

        public override void CurrentSelectionBuild()
        {
            var item = new BreadCrumbsModel()
            {
                NameLink = _links.Last().NameLink
            };
            Container.AddElement(item);
        }

        #region construct link

        protected void CategoryTree(IEnumerable<IBreadCrumbsCategoryModel> links)
        {

            foreach (var link in links)
            {
                var item = new BreadCrumbsModel()
                {
                    Href = UrlHelper.Action("Category", "Catalog", new { type = link.TypeHref, id = link.CategoryId }),
                    NameLink = link.NameLink
                };
                Container.AddElement(item);
            }
        }

        protected void CurrentCatalog(IBreadCrumbsCategoryModel category)
        {
            var item = new BreadCrumbsModel()
            {
                Href = UrlHelper.Action("Categories", "Catalog", new { type = category.TypeHref }),
                NameLink = category.TypeName
            };
            Container.AddElement(item);
        }

        #endregion
        #region construct the tree relation for the category

        protected IEnumerable<IBreadCrumbsCategoryModel> CategoryWithChild(int id)
        {
            var lang = GetCurrentLanguage();
            var categoryRepository = IoC.Resolve<ICategoryRepository>();
           
            var data = categoryRepository.EnableProxy<ICategoryRepository>().GetAll()
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
            }).Reverse();



            return Mapper.DynamicMap<IEnumerable<IBreadCrumbsCategoryModel>>(result);

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

        #endregion

    }
}