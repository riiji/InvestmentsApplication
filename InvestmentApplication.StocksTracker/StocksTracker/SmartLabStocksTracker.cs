using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using InvestmentApplication.StocksTracker.StocksTracker.Abstractions;
using InvestmentsApplication.ReportScraper.Parsers;

namespace InvestmentApplication.StocksTracker.StocksTracker
{
    public class SmartLabStocksTracker : IStocksTracker
    {
        private string sharesUrl = "https://smart-lab.ru/q/shares/";
        private readonly HtmlWeb web = new HtmlWeb();

        public List<Stock> GetAllStocks()
        {
            var document = web.Load(sharesUrl);

            var tickers = document.DocumentNode
                .SelectNodes(".//span[@class='portfolio_action']")
                .Select(n => n.ParentNode.ParentNode.ChildNodes[7].InnerText)
                .ToList();

            var prices = document.DocumentNode
                .SelectNodes(".//span[@class='portfolio_action']")
                .Select(n => n.ParentNode.ParentNode.ChildNodes[15].InnerText)
                .ToList();

            var stocks = new List<Stock>();

            for (int i = 0; i < tickers.Count(); i++)
            {
                stocks.Add(new Stock()
                {
                    Ticker = tickers[i],
                    Price = FloatParser.SafeParse(prices[i]),
                    LastUpdateTime = DateTime.Now
                });
            }

            return stocks;
        }

        public Stock GetStockByTicker(string ticker)
        {
            return GetAllStocks()
                .First(s => s.Ticker == ticker);
        }
    }
}