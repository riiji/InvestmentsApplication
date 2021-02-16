using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using InvestmentApplication.StocksTracker.StocksTracker;
using InvestmentApplication.StocksTracker.StocksTracker.Abstractions;
using InvestmentsApplication.Notifier;
using InvestmentsApplication.ReportScraper.Scraper;
using InvestmentsApplication.ReportScraper.Scraper.Abstractions;
using Quartz;
using Quartz.Impl;

namespace InvestmentsApplication
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var scraper = new SmartLabReportScraper();
            var reports = scraper.ScrapeAllCompanyReports();
            
            var algorithm = new SimpleRealPriceAlgorithm();
            var tracker = new SmartLabStocksTracker();
            var stocks = tracker.GetAllStocks();

            var formingReports = new List<(float, string)>();
            foreach (var report in reports)
            {
                var calculatedPrice = algorithm.Calculate(report);
                if (calculatedPrice != null)
                {
                    var realPriceStock = stocks.Where(s => s.Ticker == report.Ticker);
                    var realPrice = realPriceStock?.Count() > 0 ? realPriceStock?.First().Price : null;

                    if (realPrice != null)
                    {
                        var interest = algorithm.CalculateInterest((float)calculatedPrice, (float)realPrice);

                        if (interest != null)
                        {
                            formingReports.Add(((float)interest, report.Ticker));
                        }
                    }
                }
            }

            formingReports = formingReports
                .OrderBy(f => f.Item1)
                .ToList();

            foreach (var report in formingReports)
            {
                Console.WriteLine($"{report.Item2} {report.Item1}");
            }
        }
    }
}
