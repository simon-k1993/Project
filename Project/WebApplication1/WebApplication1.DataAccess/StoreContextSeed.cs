using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication1.Domain.Models;

namespace WebApplication1.DataAccess
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("C:/Users/Simon/Desktop/Project/WebApplication1/WebApplication1.DataAccess/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                context.ProductBrands.AddRange(brands);
            }

            if (!context.ProductTypes.Any())
            {
                var typesData = File.ReadAllText("C:/Users/Simon/Desktop/Project/WebApplication1/WebApplication1.DataAccess/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                context.ProductTypes.AddRange(types);
            }
            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();

            if (!context.Products.Any())
            {
                var productsData = File.ReadAllText("C:/Users/Simon/Desktop/Project/WebApplication1/WebApplication1.DataAccess/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                context.Products.AddRange(products);
            }

            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}
