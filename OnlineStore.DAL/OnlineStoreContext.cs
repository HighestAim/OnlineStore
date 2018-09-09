using OnlineStore.Core.Entites;
using OnlineStore.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.DAL
{
    public class OnlineStoreContext : DbContext
    {
        public OnlineStoreContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region -- model builder configurations --
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            #endregion -- model builder configurations --
        }
    }
}
