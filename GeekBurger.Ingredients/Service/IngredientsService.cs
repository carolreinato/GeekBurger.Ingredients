using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GeekBurger.Ingredients.Contract.DTO;
using GeekBurger.Ingredients.Interface;
using GeekBurger.Ingredients.Repository;
using GeekBurger.Ingredients.Validations;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Service
{
    public class IngredientsService : IIngredientsService
    {
        private readonly IProductRepository _productRepository;
        private readonly IIngredientsRequestValidator _ingredientsRequestValidator;
        private IMapper _mapper;

        public IngredientsService(IProductRepository productRepository,
            IIngredientsRequestValidator ingredientsRequestValidator,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _ingredientsRequestValidator = ingredientsRequestValidator;
            _mapper = mapper;
        }

        public async Task<List<IngredientsResponse>> GetProductsByRestrictions(IngredientsRequest request)
        {
            ValidationResult validate = _ingredientsRequestValidator.Validate(request);
            if (!validate.IsValid)
                return null;

            var productsByStore = await _productRepository.GetProductsByStoreName(request.StoreName);

            if(productsByStore.Any())
            {
                var productsByRestriction = productsByStore.Where(x => x.Items.All(y => !request.Restrictions.Contains(y.Name))).ToList();
                if (productsByRestriction.Any())
                {
                    var response = _mapper.Map<List<IngredientsResponse>>(productsByRestriction);
                    return response;
                }
            }
            return null;
        }

        public async Task MergeProductAndIngredients(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
