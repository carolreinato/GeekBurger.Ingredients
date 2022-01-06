using GeekBurger.Ingredients.Model;
using GeekBurger.Products.Contract;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Interface
{
    public interface IProductRepository
    {
        Task<List<ProductToGet>> GetProductsByStoreName(string storeName);
        Task<IEnumerable<ProductIngredients>> GetProductIngredients(Guid productId);
        Task UpdateProductIngredients(ProductIngredients productIngredients);
    }
}
