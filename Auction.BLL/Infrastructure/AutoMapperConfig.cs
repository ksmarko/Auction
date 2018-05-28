using Auction.BLL.DTO;
using Auction.DAL.Entities;

using AutoMapper;
using Auction.DAL.Identity.Entities;

namespace Auction.BLL.Infrastructure
{
    public class AutoMapperConfig
    {
        public static void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Trade, TradeDTO>();
            cfg.CreateMap<Category, CategoryDTO>();
            cfg.CreateMap<Lot, LotDTO>();
            cfg.CreateMap<User, UserDTO>();
            cfg.CreateMap<ApplicationUser, UserDTO>();
        }
    }
}
