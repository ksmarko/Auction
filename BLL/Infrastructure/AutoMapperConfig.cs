using BLL.DTO;
using DAL.Entities;

using AutoMapper;

namespace BLL.Infrastructure
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Trade, TradeDTO>();
                cfg.CreateMap<Category, CategoryDTO>();
                cfg.CreateMap<Lot, LotDTO>();
                cfg.CreateMap<User, UserDTO>();
            });
        }
    }
}
