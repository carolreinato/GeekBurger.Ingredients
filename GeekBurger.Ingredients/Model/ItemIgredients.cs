using System;
using System.Collections.Generic;

namespace GeekBurger.Ingredients.Model
{
    public class ItemIgredients
    {
        public Guid ItemId { get; set; }
        public List<string> Ingredients { get; set; }
    }
}
