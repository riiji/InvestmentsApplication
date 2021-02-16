using System.Collections.Generic;

namespace InvestmentApplication.StocksTracker.StocksTracker.Abstractions
{
    public interface IStocksTracker
    {
        List<Stock> GetAllStocks();
        Stock GetStockByTicker(string ticker);
    }
}