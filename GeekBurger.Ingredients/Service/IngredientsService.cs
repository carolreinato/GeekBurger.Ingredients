using AutoMapper;
using GeekBurger.Ingredients.Contract.DTO;
using GeekBurger.Ingredients.Interface;
using GeekBurger.Ingredients.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Service
{
    public class IngredientsService : IIngredientsService
    {
        private readonly IProductRepository _productRepository;
        private IMapper _mapper;

        public IngredientsService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<IngredientsResponse>> GetProductsByRestrictions(IngredientsRequest request)
        {
            var productsByStore = await _productRepository.GetProductsByStoreName(request.StoreName);
            //Estoura erro quando mapeia
            //var products = _mapper.Map<List<Product>>(productsByStore);
            var itemsRetrictions = _mapper.Map<List<ItemToGet>>(request.Restrictions);

            var productsByRestriction = productsByStore.Where(x => x.Items.All(y => itemsRetrictions != x.Items));
            var response = _mapper.Map<List<IngredientsResponse>>(productsByRestriction);

            return response;
        }
    }
}
