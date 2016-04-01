using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Infrastructure.Data.Context.Store.Context;
using NUnit.Framework;

namespace FashionStore.Test.PresentationLayer.MapperEntity
{
    [TestFixture]
    public class MapperTest
    {
        [Test]
        public void MapperModel()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<OrganizationProfile>());
            //var goods = new Goods()
            //{
            //    GoodId = 10
            //};
            var mapper = config.CreateMapper();
            var goods = new ShopContext().Goods.Where(g => g.GoodId < 10);
            var dest = goods.ProjectTo<DestGood>(config);
            CollectionAssert.Contains(dest.Select(d=>d.goodId),Enumerable.Range(1,9));
        }
       
    }
    public class OrganizationProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Good, DestGood>();
            
            //Use CreateMap... Etc.. here (Profile methods are the same as configuration methods)
        }
    }

    public class Goods
    {
        public int GoodId { get; set; }
        
    }
    public class DestGood
    {
        public int goodId { get; set; }
    }
}