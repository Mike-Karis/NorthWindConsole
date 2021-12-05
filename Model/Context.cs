using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace NorthWindConsole.Model
{
    public class Context : DbContext
    {
        public DbSet<Product> products { get; set; }

        public void AddProduct(Product products)
        {
            //base.Configuration.LazyLoadingEnabled = false; 
            this.products.Add(products);
            this.SaveChanges();
        }
          public void DeleteProduct(Product products)
        {
            this.products.Remove(products);
            this.SaveChanges();
        }
         public void EditProduct(Product UpdatedProducts)
        {
            Product blog = this.products.Find(UpdatedProducts.ProductId);
            blog.ProductName = UpdatedProducts.ProductName;
            this.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
                        IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            optionsBuilder.UseSqlServer(@config["BloggingContext:ConnectionString"]);
        }
    }
}