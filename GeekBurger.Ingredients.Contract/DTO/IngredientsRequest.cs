using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Contract.DTO
{
    public class IngredientsRequest
    {
        public string StoreName { get; set; }
        public List<string> Restrictions { get; set; }
    }
}
