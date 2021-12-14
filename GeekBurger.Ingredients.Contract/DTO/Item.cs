using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Contract.DTO
{
    public class Item
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
    }
}
