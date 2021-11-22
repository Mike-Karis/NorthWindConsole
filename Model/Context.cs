using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace NorthWindConsole.Model
{
    public class Context : DbContext
    {
        public DbSet<Product> products { get; set; }

        public void AddBlog(Product products)
        {
            //base.Configuration.LazyLoadingEnabled = false; 
            this.products.Add(products);
            this.SaveChanges();
        }
          public void DeleteBlog(Product products)
        {
            this.products.Remove(products);
            this.SaveChanges();
        }
         public void EditBlog(Product UpdatedProducts)
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