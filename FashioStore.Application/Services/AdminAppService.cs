using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FashionStore.Application.Services.Interfaces;
using FashionStore.Domain.Interfaces.Repository;

namespace FashionStore.Application.Services
{
    public class AdminAppService : IAdminAppService
    {
        private ICategoryRepository _categoryRepository;
        private IGoodsRepository _goodsRepository;

        public AdminAppService(ICategoryRepository categoryRepository, IGoodsRepository goodsRepository)
        {
            _categoryRepository = categoryRepository;
            _goodsRepository = goodsRepository;
        }

        #region goods

        public async Task<TResult> GetGoodsByPageAsync<TResult>(int categoryId, int page, int perPage)
        {
            var totalCount = await Task.Run(() => _goodsRepository.DisabledProxy<IGoodsRepository>()
                .GetAll().Count(g => g.CategoryId == categoryId));

            var data = await Task.Run(() => _goodsRepository.DisabledProxy<IGoodsRepository>().GetAll()
                .Where(g => g.CategoryId == categoryId)
                .Select(g => new
                {
                    g.GoodId,
                    g.GoodNameEn,
                    g.GoodNameRu,
                    Discount = g.Discount ?? 0,
                    g.DateCreate,

                    g.PriceUsd,
                    photos = g.Image.FirstOrDefault(),

                })
                .OrderBy(g => g.GoodId).Skip(perPage*(page - 1)).Take(() => perPage)
                .GroupBy(g => new {})
                .Select(gr => new
                {
                    totalPagesCount = Math.Ceiling((double) totalCount/perPage),
                    items = gr.Select(g => new
                    {
                        g.GoodId,
                        g.GoodNameEn,
                        g.GoodNameRu,
                        g.Discount,
                        g.DateCreate,

                        g.PriceUsd,
                        g.photos
                    })
                }).FirstOrDefault());

            if (data == null)
            {
                return default(TResult);
            }
            return Mapper.DynamicMap<TResult>(data);
        }

        public async Task<TResult> FullInfoGoodsAsync<TResult>(int id)
        {
            var data = await
                _goodsRepository.DisabledProxy<IGoodsRepository>().GetAll()
                    .Where(g => g.GoodId == id).Select(g => new
                    {
                        g.GoodId,
                        g.CategoryId,
                        typeId = g.Category.TypeId,
                        Discount = g.Discount ?? 0,

                        g.GoodNameEn,
                        g.GoodNameRu,
                        g.PriceUsd,
                        photos = g.Image.Select(i => i.ImagePath),
                        clsn =
                            g.ClassificationGoods.Select(c => new {c.ClassificationId, c.ColorId, c.SizeId, c.CountGood})
                    }).FirstOrDefaultAsync();

            return Mapper.DynamicMap<TResult>(data);
        }

        #endregion

        


        public async Task<IEnumerable<TResult>> AllCategoryByTypeAsync<TResult>()
        {
            var data = await Task.Run(
                () => _categoryRepository.DisabledProxy<ICategoryRepository>().GetAll()
                .GroupBy(c => new { c.TypeId, c.Type.TypeNameEn })
                .Select(g => new
                {
                    id = g.Key.TypeId,
                    name = g.Key.TypeNameEn,
                    category = g.Select(c => new
                    {
                        c.CategoryId,
                        c.Name.CategoryNameEn
                    }).OrderBy(c=>c.CategoryNameEn)
                }).OrderBy(i=>i.name));

            return DynamicMap<TResult>(data);
        }

        #region Helper methods

        private IEnumerable<TResult> DynamicMap<TResult>(IQueryable data)
        {
            return Mapper.DynamicMap<IEnumerable<TResult>>(data);
        }

        #endregion

    }
}