using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Contract.DTO
{
    public class IngredientsResponse
    {
        public Guid ProductId { get; set; }
        public List<ItemToGet> Ingredients { get; set; }
    }
}
