using System;

namespace InvestmentApplication.StocksTracker
{
    public class Stock
    {
        public int Id { get; set; }
        public string Ticker { get; set; }
        public float? Price { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}