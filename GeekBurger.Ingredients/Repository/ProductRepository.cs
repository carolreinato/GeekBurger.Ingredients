using AutoMapper;
using Dapper;
using GeekBurger.Ingredients.Contract.DTO;
using GeekBurger.Ingredients.Interface;
using GeekBurger.Ingredients.Model;
using GeekBurger.Products.Contract;
using Microsoft.Azure.ServiceBus;
using Microsoft.Data.Sqlite;
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
        public ProductRepository()
        {
            _ = CreateTableProductsIngredient();
        }

        private async Task CreateTableProductsIngredient()
        {
            using SqliteConnection con = new SqliteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            using SqliteCommand command = con.CreateCommand();
            con.Open();
            command.CommandText = "SELECT name FROM sqlite_master WHERE name='ProductIngredient'";
            var name = command.ExecuteScalar();

            // check account table exist or not 
            // if exist do nothing 
            if (name != null && name.ToString() == "ProductIngredient")
                return;
            // acount table not exist, create table and insert 
            command.CommandText = "CREATE TABLE ProductIngredient (ProductID VARCHAR(80), StoreName VARCHAR(20), ItemIgredients VARCHAR(2000))";
            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<ProductToGet>> GetProductsByStoreName(string storeName)
        {
            List<ProductToGet> products = new();

            using (var client = new HttpClient())
            {
                //UriBuilder builder = new UriBuilder($"https://geekburgerproducts20211212122844.azurewebsites.net/api/products/{storeName}");
                UriBuilder builder = new UriBuilder($"https://geekburger-products.azurewebsites.net/api/products?storeName={storeName}");
                //builder.Query = storeName;

                var response = await client.GetAsync(builder.Uri);
                if (response.StatusCode != HttpStatusCode.OK)
                    return products;
                var json = response.Content.ReadAsStringAsync().Result;
                products = JsonConvert.DeserializeObject<List<ProductToGet>>(json);
                return products;
            }

        }

        public async Task<IEnumerable<ProductIngredients>> GetProductIngredients(Guid productId)
        {
            using var connection = new SqliteConnection("Data Source=ProductsIngredients.db");

            var query = await connection.QueryAsync<ProductIngredients>(@"
                SELECT *
                FROM ProductIngredient
                WHERE ProductId = @productId
                ", new { productId = productId });

            return query;
        }

        public async Task UpdateProductIngredients(ProductIngredients productIngredients)
        {
            using var connection = new SqliteConnection("Data Source=ProductsIngredients.db");

            await connection.ExecuteAsync(@"INSERT INTO Product (ProductID, StoreName, ItemIgredients)
                                            VALUES (@Name, @Description);", new { productId = productIngredients.ProductId, 
                                                                                  storeName = productIngredients.StoreName, 
                                                                                  itemIgredients = JsonConvert.SerializeObject(productIngredients.ItemIgredients)
            });
        }

        public Task MergeProductAndIngredients(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
