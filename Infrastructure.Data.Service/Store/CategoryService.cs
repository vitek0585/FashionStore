using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infastructure.Data.Service.Store.Common;
using FashionStore.Service.Interfaces.Services;
using FashionStore.Service.Interfaces.UoW;
using LinqKit;

namespace FashionStore.Infastructure.Data.Service.Store
{
    public class CategoryService : EntityService<Category>, ICategoryService
    {
        private ICategoryRepository _category;
        private IGoodsRepository _goods;

        private ICategoryTypeRepository _typeRepository;
        private IExchangeRatesService _exchangeRates;
        public CategoryService(IUnitOfWorkStore unitOfWork, ICategoryRepository repository, ICategoryTypeRepository typeRepository,
            IExchangeRatesService exchangeRates, IGoodsRepository goods)
            : base(unitOfWork, repository)
        {
            _goods = goods;
            _exchangeRates = exchangeRates;
            _category = repository;
            _typeRepository = typeRepository;
        }
        public IEnumerable<TResult> GetRandom<TResult>(int count, string lang) where TResult : class
        {
            var propNameCategory = GetPropertyName<CategoryName>(lang, (g) => g.CategoryNameRu);
            var rand = new Random();
            var data = _category.DisabledProxy<ICategoryRepository>().GetAll()
                .Include(c => c.Name)
                .Include(c => c.Type)
                .AsExpandable().Where(c => c.ParentId == null)
                .AsEnumerable()
                .Select(c => new
                {
                    categoryId = c.CategoryId,
                    categoryName = propNameCategory.GetValue(c.Name),
                    photo = c.ImagePath,
                    typeByHref = c.Type.TypeNameEn

                }).OrderBy(c => rand.Next());


            return ReturnValue<TResult>(data.Take(count));

        }
        public IEnumerable<TResult> AllCategory<TResult>(string lang) where TResult : class
        {
            var data = (from type in _category.DisabledProxy<ICategoryRepository>().GetCategoryTypes()
                        join category in _category.DisabledProxy<ICategoryRepository>().GetAll() on type.TypeId equals
                            category.TypeId
                        where category.ParentId == null
                        select new
                        {
                            type.TypeNameEn,
                            type.TypeNameRu,
                            type.TypeId,
                            TypeByHref = category.Type.TypeNameEn,
                            category.CategoryId,
                            category.Name.CategoryNameEn,
                            category.Name.CategoryNameRu,

                        }).AsEnumerable()
                       .Select(g => ToProjection(g, lang))
                       .GroupBy(c => new { c.TypeId, c.typeName, c.TypeByHref })
                       .Select(gr => new
                       {
                           gr.Key.TypeId,
                           gr.Key.typeName,
                           gr.Key.TypeByHref,
                           items = gr.Select(k => new { k.CategoryId, k.CategoryName })
                       });


            return Mapper.DynamicMap<IEnumerable<TResult>>(data); //ReturnValue<TResult>(data);

        }

        public IEnumerable<TResult> CategoryAndChild<TResult>(int id, string lang) where TResult : class
        {
            var data = _category.EnableProxy<ICategoryRepository>().GetAll()
                .Where(c => c.CategoryId == id).Include(c => c.Parent).Include(c => c.Parent.Parent)
                .Include(c => c.Parent.Parent.Parent).Include(c => c.Name)
                .Include(c => c.Type)
                .FirstOrDefault();

            if (data == null)
                throw new ArgumentException(string.Format("Category by id {0} not found", id));

            var result = GetChild(data).Select(c => new
            {
                c.CategoryId,
                NameLink =
                    GetPropertyName<CategoryName>(lang, cn => cn.CategoryNameRu).GetValue(c.Name),
                TypeName = GetPropertyName<CategoryType>(lang, cn => cn.TypeNameRu).GetValue(c.Type),
                TypeHref = c.Type.TypeNameEn
            });
            return ReturnValue<TResult>(result);
        }
       
        public TResult GetCategoriesByType<TResult>(string type, string lang) where TResult : class
        {
            var data = QueryForType(lang, c => c.Type.TypeNameEn == type && c.ParentId == null);

            return Mapper.DynamicMap<TResult>(data);
        }
        public TResult GetCategoriesSale<TResult>(string type, string lang, int discount) where TResult : class
        {
            var data = QueryForType(lang, c => c.Type.TypeNameEn == type && c.ParentId == null && c.Goods.Any(g => g.Discount >= discount));
            return Mapper.DynamicMap<TResult>(data);
        }
        public TResult GetCategoryByCulture<TResult>(string type, int id, string lang)
        {
            var ids = _category.GetCategoryAndChildId(id);
            var data = (from c in _category.DisabledProxy<ICategoryRepository>().GetAll()
                        where ids.Contains(c.CategoryId) && c.Type.TypeNameEn == type
                        select new
                        {
                            c.CategoryId,
                            c.Name.CategoryNameRu,
                            c.Name.CategoryNameEn,
                            c.Type.TypeNameEn,
                            c.Type.TypeNameRu,
                        }).AsEnumerable().GroupBy(c => new { }).Select(c => new
                        {
                            CategoryId = id,
                            TypeByHref = c.First(cat => cat.CategoryId == id).TypeNameEn,
                            CategoryName = GetNameByCurrentLangForDynamicType<CategoryName>(c.First(cat => cat.CategoryId == id),
                                lang, ca => ca.CategoryNameRu),
                            typeName = GetNameByCurrentLangForDynamicType<CategoryType>(c.First(cat => cat.CategoryId == id),
                                lang, ca => ca.TypeNameEn),

                            Childrens = c.Where(cat => cat.CategoryId != id).Select(child => new
                            {
                                CategoryId = child.CategoryId,
                                TypeByHref = child.TypeNameEn,
                                CategoryName = GetNameByCurrentLangForDynamicType<CategoryName>(child, lang, ca => ca.CategoryNameRu),
                                typeName = GetNameByCurrentLangForDynamicType<CategoryType>(child, lang, ca => ca.TypeNameEn)
                            })

                        }).FirstOrDefault();

            return Mapper.DynamicMap<TResult>(data);
        }
        public int? GetTypeIdByName(string type)
        {
            return _typeRepository.GetByExpressionSelect(c => c.TypeNameEn == type, c => c.TypeId);
        }
        public TResult GetInformationAboutCategory<TResult>(int category, string currentCurrency, string lang)
        {
            var ids = _category.GetCategoryAndChildId(category);
            decimal onehundred = 100;
            var data = (from good in _goods.DisabledProxy<IGoodsRepository>().GetAll()
                        join cat in _category.DisabledProxy<ICategoryRepository>().GetAll() on good.CategoryId equals
                            cat.CategoryId
                        where ids.Contains(good.CategoryId.Value)
                        group good by good.GoodId into g
                        select new
                        {
                            c = g.SelectMany(c => c.ClassificationGoods, (gd, clg) => new
                            {
                                id = clg.ColorId,
                                clg.Color.ColorNameEn,
                                clg.Color.ColorNameRu,
                                idS = clg.SizeId,
                                clg.Size.SizeName
                            }),
                            min = g.Min(i => i.PriceUsd - (i.Discount ?? 0) / onehundred * i.PriceUsd),
                            max = g.Max(i => i.PriceUsd - (i.Discount ?? 0) / onehundred * i.PriceUsd)
                        }).AsEnumerable()
                .GroupBy(g => new { })
                .Select(gr => new
                {
                    colors = gr.SelectMany(g => g.c.Select(c =>
                        new
                        {
                            c.id,
                            color = c.ColorNameEn,
                            name = GetNameByCurrentLangForDynamicType<Color>(c, lang, col => col.ColorNameEn)
                        })).Distinct(),
                    sizes = gr.SelectMany(g => g.c.Select(c => new
                        {
                            id = c.idS,
                            name = c.SizeName
                        })).Distinct(),

                    min = gr.Min(p => p.min),
                    max = gr.Max(p => p.max),
                    ExchangeRates = Math.Ceiling(_exchangeRates.GetActualRateUsdWithUpdateIfNotExist())
                }).FirstOrDefault();

            return Mapper.DynamicMap<TResult>(data);
        }

        #region Additional Methods

        private dynamic ToProjection(dynamic result, string lang)
        {
            return new
            {
                result.TypeId,
                typeName = GetNameByCurrentLangForDynamicType<CategoryType>((object)result, lang, c => c.TypeNameEn),
                result.CategoryId,
                result.TypeByHref,
                CategoryName =
                    GetNameByCurrentLangForDynamicType<CategoryName>((object)result, lang, c => c.CategoryNameRu),
            };
        }
        private dynamic ToProjectionWithPhoto(dynamic result, string lang)
        {
            return new
            {
                result.TypeId,
                TypeName = GetNameByCurrentLangForDynamicType<CategoryType>((object)result, lang, ca => ca.TypeNameRu),
                result.CategoryId,
                result.photo,
                result.TypeByHref,
                CategoryName =
                    GetNameByCurrentLangForDynamicType<CategoryName>((object)result, lang, ca => ca.CategoryNameRu),
            };
        }
        private dynamic QueryForType(string lang, Expression<Func<Category, bool>> pred)
        {
            return _category.DisabledProxy<ICategoryRepository>()
                .GetAll().Where(pred).AsExpandable()
                .Select(c => new
                {
                    c.Type.TypeNameEn,
                    c.Type.TypeNameRu,
                    c.TypeId,

                    TypeByHref = c.Type.TypeNameEn,

                    c.CategoryId,
                    c.Name.CategoryNameEn,
                    c.Name.CategoryNameRu,
                    photo = c.ImagePath,

                }).AsEnumerable().Select(c => ToProjectionWithPhoto(c, lang))
                .GroupBy(c => new { c.TypeName, c.TypeId, c.TypeByHref }).Select(gr => new
                {
                    gr.Key.TypeId,
                    gr.Key.TypeName,
                    gr.Key.TypeByHref,
                    items = gr.Select(t => new { t.CategoryId, t.photo, t.CategoryName })
                }).FirstOrDefault();
        }
        private IEnumerable<TResult> ReturnValue<TResult>(IEnumerable<dynamic> data) where TResult : class
        {
            return Mapper.DynamicMap<IEnumerable<TResult>>(data);
        }

        private IEnumerable<Category> GetChild(Category data)
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