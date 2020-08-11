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
        }
    }
}