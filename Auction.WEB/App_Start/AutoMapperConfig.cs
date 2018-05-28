using AutoMapper;
using BLL.DTO;
using Auction.WEB.Models;

namespace Auction.WEB.App_Start
{
    public class AutoMapperConfig
    {
        public static void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<LotDTO, LotModel>()
                .ForMember(dst => dst.Creator, map => map.MapFrom(src => src.User.Name))
                .ForMember(dst => dst.Category, map => map.MapFrom(src => src.Category.Name));

            cfg.CreateMap<LotModel, LotDTO>()
                .ForMember(x => x.IsVerified, opt => opt.Ignore())
                .ForMember(x => x.Category, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore());

            cfg.CreateMap<TradeDTO, TradeModel>()
                .ForMember(dst => dst.DaysLeft, map => map.MapFrom(src => src.TradeEnd.Subtract(src.TradeStart).Days));
        }
    }
}