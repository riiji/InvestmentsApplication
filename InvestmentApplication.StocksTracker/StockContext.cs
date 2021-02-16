using Microsoft.EntityFrameworkCore;

namespace InvestmentApplication.StocksTracker
{
    public class StockContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Server=localhost; Port=5432; Database=Stocks;User ID=postgres;Password=123123;Integrated Security=true;Pooling=true");
        }

        public DbSet<Stock> Stocks { get; set; }
    }
}