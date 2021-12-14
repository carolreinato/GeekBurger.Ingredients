using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Model
{
    public class Product
    {
        [ForeignKey("StoreId")]
        public Store Store { get; set; }
        [Key]
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public ICollection<Item> Items { get; set; } = new List<Item>();

    }
}
