using System;
using System.Collections.Generic;

namespace GeekBurger.Ingredients.Contract.DTO
{
    public class IngredientsResponse
    {
        public Guid ProductId { get; set; }
        public List<string> Ingredients { get; set; }
    }
}
