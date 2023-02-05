using KindBee.DB.DBModels;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace KindBee.DB
{
    public class KindBeeDBContext: DbContext
    {
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Basket> Baskets { get; set; } = null!;
        public DbSet<Position> Positions { get; set; } = null!;

        public KindBeeDBContext()
        {
            //Database.EnsureDeleted();   // удаляем бд со старой схемой
            //Database.EnsureCreated();   // создаем бд с новой схемой
        }
        public KindBeeDBContext(DbContextOptions<KindBeeDBContext> options) : base(options)
        {
            //Database.EnsureDeleted();   // удаляем бд со старой схемой
            //Database.EnsureCreated();   // создаем бд с новой схемой
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Data Source=DESKTOP-VHEI76T;Initial Catalog=KindBee;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
