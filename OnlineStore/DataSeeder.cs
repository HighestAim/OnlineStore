using OnlineStore.DAL;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;
using OnlineStore.Core.Entites;
using OnlineStore.Core.Models.JsonModels;

namespace OnlineStore.API
{
    public static class DataSeeder
    {
        private static OnlineStoreContext _context;
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
           .GetRequiredService<IServiceScopeFactory>()
           .CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<OnlineStoreContext>();
                _context = context;
                context.Database.Migrate();
                MigrateStaticData();
            }
        }

        private static void MigrateStaticData()
        {
            // Categories Migration From Json Static File
            MigrateCategories();
            // Products Migration From Json Static File
            MigrateProducts();
        }

        private static void MigrateCategories()
        {
            using (var reader = new StreamReader("StaticData/Categories.json"))
            {
                var jsonData = reader.ReadToEnd();
                var categoryJsonData = JsonConvert.DeserializeObject<IEnumerable<CategoryJsonModel>>(jsonData);

                var dbCategories = _context.Categories.Select(category => category.SeederId).ToList();

                categoryJsonData = categoryJsonData.Where(x => !dbCategories.Contains(x.SeederId));

                if (categoryJsonData.Any())
                {
                    var categories = categoryJsonData.Select(item => new Category
                    {
                        Name = item.Name,
                        SeederId = item.SeederId
                    });

                    _context.Categories.AddRange(categories);
                    _context.SaveChanges();
                }
            }
        }

        private static void MigrateProducts()
        {
            using (var reader = new StreamReader("StaticData/Products.json"))
            {
                var jsonData = reader.ReadToEnd();
                var productJsonData = JsonConvert.DeserializeObject<IEnumerable<ProductJsonModel>>(jsonData);

                var dbProducts = _context.Products.Select(product => product.SeederId).ToList();

                productJsonData = productJsonData.Where(x => !dbProducts.Contains(x.SeederId));

                if (productJsonData.Any())
                {
                    var categories = _context.Categories.Where(category => productJsonData.Any(product => product.CategorySeederId == category.SeederId))
                                                     .Select(category => new
                                                     {
                                                         category.Id,
                                                         category.SeederId
                                                     }).ToList();

                    var products = productJsonData.Select(product => new Product
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Url = product.Url,
                        SeederId = product.SeederId,
                        CategoryId = categories.First(item => item.SeederId == product.CategorySeederId).Id
                    });

                    _context.Products.AddRange(products);

                    _context.SaveChanges();
                }
            }

        }
    }
}
