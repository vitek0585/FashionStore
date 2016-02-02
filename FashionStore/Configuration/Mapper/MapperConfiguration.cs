using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Models.Order;

namespace FashionStore.Configuration.Mapper
{
    public class MapperConfig
    {
        
        public static void SetupMap()
        {
            AutoMapper.Mapper.CreateMap<UserOrderModel, SalePos>().
                ForMember(s => s.Price, opt => opt.MapFrom(g => g.PriceUsd)).
                ForMember(s => s.CountGood, opt => opt.MapFrom(g => g.CountGood));

            //Mapper.CreateMap<GoodFileModel, Good>();

            //Mapper.CreateMap<Good, GoodHome>().ForMember(d => d.PhotoPath, opt => opt.MapFrom(g => GetPhoto(g))).
            //    ForMember(g=>g.GoodCount,opt=>opt.MapFrom(o=>o.ClassificationGoods.Sum(c=>c.CountGood)));
            AutoMapper.Mapper.CreateMap<UserOrderModel, ClassificationGood>();

        }

    }
}