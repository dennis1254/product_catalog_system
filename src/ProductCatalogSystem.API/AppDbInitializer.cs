using ProductCatalogSystem.Entities;

namespace ProductCatalogSystem.API
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder application)
        {
            using(var serviceScope = application.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                
                context.Database.EnsureCreated();

                //if (!context.Products.Any())
                //{
                //    var products = new List<Product>()
                //    {
                //        new Product{Id = 1,},
                //        
                //    };
                //    context.Clients.AddRange(products);
                //}

                context.SaveChanges();
                
            }
        }
    }
}
