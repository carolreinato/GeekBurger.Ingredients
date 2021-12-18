using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Model
{
    public class ProductIngredients
    {
        public Guid ProductId { get; set; }
        public string StoreName { get; set; }
        public List<ItemIgredients> ItemIgredients { get; set; }
    }
}
