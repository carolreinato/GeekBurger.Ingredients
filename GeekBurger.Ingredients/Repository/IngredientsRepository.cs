using GeekBurger.Ingredients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Repository
{
    public class IngredientsRepository
    {
        private IngredientsDbContext _context;

        public IngredientsRepository(IngredientsDbContext context)
        {
            _context = context;
        }

        public List<Product> GetIngredientsByProductName(List<Item> ingredients, string storeName)
        {
            var productsByRestriction = _context.Products?.Where(x => !ingredients.Any(y => y == x.Items) && x.Store.Name == storeName).ToList();
            return productsByRestriction;
        }
    }
}
