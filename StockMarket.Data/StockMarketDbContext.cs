using Microsoft.EntityFrameworkCore;
using StockMarket.Domain;

/* 
 * Required Steps:
 * Set StockMarket.Data as Startup Project
 * Open Package Manager Console
 * Change default Project in Package Manager Console to StockMarket.Data
 * Commands:
 * Create the First Migration Using "add-migration init" Command
 * Create Migration Using "add-migration" Command
 * Remove Migration Using "remove-migration" Command
 * Push Migration to Database Using "update-database" Command
 * Rollback Migration using "update-database 0" Command
*/

namespace StockMarket.Data
{
    public class StockMarketDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Trade> Trades { get; set; }

        public StockMarketDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(b =>
            {
                b.Property(o => o.Id).ValueGeneratedNever();
                b.Property(o => o.Side);
                b.Property(o => o.Price).HasColumnType("Money");
                b.Property(o => o.Quantity).HasColumnType("Money");
                b.Property("version").IsRowVersion();
            });

            modelBuilder.Entity<Trade>(b =>
            {
                b.Property(t => t.Id).ValueGeneratedNever();
                b.HasOne<Order>().WithMany().IsRequired().HasForeignKey(t => t.SellOrderId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<Order>().WithMany().IsRequired().HasForeignKey(t => t.BuyOrderId).OnDelete(DeleteBehavior.Restrict);
                b.Property(t => t.Price).HasColumnType("Money");
                b.Property(t => t.Quantity).HasColumnType("Money");
                b.Property("version").IsRowVersion();
            });
        }
    }
}
//goozoo