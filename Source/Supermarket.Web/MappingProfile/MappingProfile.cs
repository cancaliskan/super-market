using AutoMapper;

using Supermarket.Domain.Entities;
using Supermarket.Web.Models;

namespace Supermarket.Web.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductViewModel, Product>();

            CreateMap<ProductDetail, Product>();
            CreateMap<Product, ProductDetail>();

            CreateMap<BasketViewModel, Basket>();
            CreateMap<Basket, BasketViewModel>();

            CreateMap<SalesInformation, SalesInformationViewModel>();
            CreateMap<SalesInformationViewModel, SalesInformation>();

            CreateMap<OrderProductInformation, OrderProductInformationViewModel>();
            CreateMap<OrderProductInformationViewModel, OrderProductInformation>();
        }
    }
}