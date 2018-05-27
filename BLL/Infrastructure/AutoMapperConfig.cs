using BLL.DTO;
using DAL.Entities;

using AutoMapper;
using DAL.Identity.Entities;

namespace BLL.Infrastructure
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
