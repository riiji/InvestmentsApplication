using InvestmentApplication.StocksTracker.Repositories.Abstractions;

namespace InvestmentApplication.StocksTracker.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly StockContext context;

        public StockRepository(StockContext context)
        {
            this.context = context;
        }

        public void AddStock(Stock stock)
        {
            context.Add(stock);
            context.SaveChanges();
        }

        public void UpdateStock(Stock stock)
        {
            context.Update(stock);
            context.SaveChanges();
        }
    }
}