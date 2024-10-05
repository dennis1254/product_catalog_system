
using AutoMapper;
using ProductCatalogSystem.Core.Entities;
using ProductCatalogSystem.Core.Models;
using ProductCatalogSystem.Entities;

namespace ProductCatalogSystem
{
    public class Automapper : Profile
    {
        public Automapper()
        {
            CreateMap<CreateProductRequest, Product>().ReverseMap();
            CreateMap<CreateInventoryRequest, Inventory>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<LoginRequest, User>();
            CreateMap<RegisterRequest, User>()
                .ForMember(d => d.UserName, map => map.MapFrom(s => s.Email));

            //reverse is for biderectional mapping no need checking for source and destination

        }
    }
}
