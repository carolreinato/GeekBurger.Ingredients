using AutoMapper;
using GeekBurger.Ingredients.Contract.DTO;
using GeekBurger.Products.Contract;
using System.Linq;

namespace GeekBurger.Ingredients.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductToGet, IngredientsResponse>()
                .ForMember(x => x.ProductId, y => y.MapFrom(a => a.ProductId))
                .ForMember(x => x.Ingredients, y => y.MapFrom(a => a.Items.Select(b => b.Name)));
        }
    }
}
