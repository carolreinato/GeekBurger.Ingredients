using AutoMapper;
using GeekBurger.Ingredients.Contract.DTO;
using GeekBurger.Ingredients.Interface;
using GeekBurger.Products.Contract;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Repository
{
    public class ProductRepository : IProductRepository
    {
        //public string Url { get; set; }

        //public ProductRepository(string url)
        //{
        //    Url = url;
        //}

        public async Task<List<ProductToGet>> GetProductsByStoreName(string storeName)
        {
            List<ProductToGet> products = new();

            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder($"https://geekburgerproducts20211212122844.azurewebsites.net/api/products/{storeName}");
                builder.Query = storeName;

                products = await client.GetFromJsonAsync<List<ProductToGet>>(builder.Uri);
            }

            return products;
        }
    }
}
