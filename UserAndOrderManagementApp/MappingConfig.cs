using AutoMapper;
using UserAndOrderManagement.Models;
using UserAndOrderManagement.Models.Dto;
using UserAndOrderManagementApp.Models.Dto;

namespace UserAndOrderManagement
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();

            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();

            CreateMap<Order, CreateOrderDto>().ReverseMap();
            CreateMap<Order, UpdateOrderDto>().ReverseMap();

            CreateMap<ApplicationUser, UserDto>().ReverseMap();
        }
    }
}
