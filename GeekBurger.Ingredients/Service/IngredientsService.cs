using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GeekBurger.Ingredients.Contract.DTO;
using GeekBurger.Ingredients.Interface;
using GeekBurger.Ingredients.Model;
using GeekBurger.Ingredients.Repository;
using GeekBurger.Ingredients.Validations;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            List<ItemIgredients> forbiddenItens = new();

            var productsByStore = await _productRepository.GetProductsByStoreName(request.StoreName);

            if (productsByStore.Any())
            {
                foreach (var product in productsByStore)
                {
                    var productIngredients = await _productRepository.GetProductIngredients(product.ProductId);
                    foreach (var itemIngredient in productIngredients.Select(x => x.ItemIgredients))
                    {
                        forbiddenItens = itemIngredient
                            .Where(x => x.Ingredients.All(y => request.Restrictions.Contains(y)))
                            .ToList();
                    }
                }
                var productsByRestriction = productsByStore
                    .Where(x => x.Items
                        .All(y => forbiddenItens
                            .Select(o => o.ItemId)
                            .Contains(y.ItemId)))
                    .ToList();

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
            try
            {
                var productsPaulista = await _productRepository.GetProductsByStoreName("Paulista");
                var productsMorumbi = await _productRepository.GetProductsByStoreName("Morumbi");

                dynamic item = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(message.Body));

                var products = productsPaulista.Where(x => x.Items.Any(y => y.Name == item.ItemName)).ToList();

                products.AddRange(productsMorumbi.Where(x => x.Items.Any(y => y.Name == item.ItemName)).ToList());


                foreach (var p in products)
                {
                    var existentProduct = (await _productRepository.GetProductIngredients(p.ProductId)).FirstOrDefault();

                    if (existentProduct != null)
                    {
                        existentProduct.ItemIgredients.Add(new ItemIgredients()
                        {
                            Ingredients = item.Ingredients,
                            ItemId = item.ItemName
                        });
                        await _productRepository.UpdateProductIngredients(existentProduct);
                    }
                    else
                    {
                        await _productRepository.UpdateProductIngredients(new ProductIngredients()
                        {
                            ProductId = p.ProductId,
                            StoreName = p.StoreId.ToString() == "8048e9ec-80fe-4bad-bc2a-e4f4a75c834e" ? "Paulista" : "Morumbi",
                            ItemIgredients = new List<ItemIgredients>() { new ItemIgredients()
                                                                            {
                                                                                Ingredients = item.Ingredients,
                                                                                ItemId = item.ItemName
                                                                            }
                                                                        }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
