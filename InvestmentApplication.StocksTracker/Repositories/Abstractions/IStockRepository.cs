namespace InvestmentApplication.StocksTracker.Repositories.Abstractions
{
    public interface IStockRepository
    {
        public void AddStock(Stock stock);
        public void UpdateStock(Stock stock);
    }
}