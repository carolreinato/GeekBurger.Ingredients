using AutoMapper;
using GeekBurger.Ingredients.Contract.DTO;
using GeekBurger.Ingredients.Interface;
using GeekBurger.Products.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Repository
{
    public class ProductRepository : IProductRepository
    {
        public async Task<List<ProductToGet>> GetProductsByStoreName(string storeName)
        {
            List<ProductToGet> products = new();

            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder($"https://geekburgerproducts20211212122844.azurewebsites.net/api/products/{storeName}");
                builder.Query = storeName;

                var response = await client.GetAsync(builder.Uri);
                if (response.StatusCode != HttpStatusCode.OK)
                    return products;
                var json = response.Content.ReadAsStringAsync().Result;
                products = JsonConvert.DeserializeObject<List<ProductToGet>>(json);
                return products;
            }

        }
    }
}
