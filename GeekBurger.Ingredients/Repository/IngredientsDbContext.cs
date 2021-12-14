using GeekBurger.Ingredients.Model;
using Microsoft.EntityFrameworkCore;

namespace GeekBurger.Ingredients.Repository
{
    public class IngredientsDbContext : DbContext
    {
        public IngredientsDbContext(DbContextOptions<IngredientsDbContext> options)
        : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }

        public DbSet<Item> Items { get; set; }

    }
}
