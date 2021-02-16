using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using InvestmentsApplication.ReportScraper.Extensions;
using InvestmentsApplication.ReportScraper.Parsers;
using InvestmentsApplication.ReportScraper.Scraper.Abstractions;

namespace InvestmentsApplication.ReportScraper.Scraper
{
    public class SmartLabReportScraper : IReportScraper
    {
        private string baseUrl = "https://smart-lab.ru";
        private string fundamentalAnalysisUrl = "https://smart-lab.ru/q/shares_fundamental";
        private readonly HtmlWeb web = new HtmlWeb();
        private (string, HtmlDocument) cachedDocument;

        public List<CompanyReport> ScrapeAllCompanyReports()
        {
            var allUrls = GetAllFundamentalAnalysisUrls();
            return allUrls.Select(ScrapeByUrl).ToList();
        }

        public CompanyReport ScrapeByUrl(string url)
        {
            var reportsWithLtm = ScrapeYearReportsWithLtm(url);
            var document = GetCachedDocument(url);

            if (IsLtm(document))
            {
                return new CompanyReport()
                {
                    Name = GetName(document),
                    Ticker = GetTicker(document),
                    YearReports = reportsWithLtm.SkipLast(1).ToList(),
                    LtmReport = reportsWithLtm.Last()
                };
            }
            else
            {
                return new CompanyReport()
                {
                    Name = GetName(document),
                    Ticker = GetTicker(document),
                    YearReports = reportsWithLtm,
                    LtmReport = null
                };
            }
        }

        private List<YearReport> ScrapeYearReportsWithLtm(string url)
        {
            var reports = new List<YearReport>();
            var document = GetCachedDocument(url);

            var dates = SafeParseDateTimeRow(document, "date");
            var revenues = SafeParseFloatRow(document, "revenue");
            var netIncomes = SafeParseFloatRow(document, "net_income");
            var commonShares = SafeParseFloatRow(document, "common_share");
            var numberOfShares = SafeParseFloatRow(document, "number_of_shares");
            var marketCapitals = SafeParseFloatRow(document, "market_cap");
            var dividendPayouts = SafeParseFloatRow(document, "dividend_payout");
            var dividends = SafeParseFloatRow(document, "dividend");
            var opexes = SafeParseFloatRow(document, "opex");
            var assets = SafeParseFloatRow(document, "assets");
            var eps = SafeParseFloatRow(document, "eps");
            var roe = SafeParsePercentRow(document, "roe");
            var roa = SafeParsePercentRow(document, "roa");
            var pe = SafeParseFloatRow(document, "p_e");
            var ps = SafeParseFloatRow(document, "p_s");
            var pbv = SafeParseFloatRow(document, "p_bv");
            var dept = SafeParseFloatRow(document, "debt");

            try
            {


                for (int i = 0; i < GetYearCount(document); i++)
                {
                    reports.Add(new YearReport()
                    {
                        Date = dates?[i],
                        Revenue = revenues?[i],
                        NetIncome = netIncomes?[i],
                        CommonShare = commonShares?[i],
                        NumberOfShares = numberOfShares?[i],
                        MarketCapital = marketCapitals?[i],
                        DividendPayout = dividendPayouts?[i],
                        Dividend = dividends?[i],
                        Opex = opexes?[i],
                        Assets = assets?[i],
                        Eps = eps?[i],
                        Roe = roe?[i],
                        Roa = roa?[i],
                        Pe = pe?[i],
                        Ps = ps?[i],
                        Pbv = pbv?[i],
                        Dept = dept?[i]
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{url}, {GetYearCount(document)}");
            }
            return reports
            .ToList();
        }

        private int GetYearCount(HtmlDocument document)
        {
            var yearCount = document.DocumentNode
                .SelectSingleNode(".//*[@class='header_row']")
                .SelectNodes("td[not(@class)]")
                .Count;

            if (IsLtm(document))
                return yearCount + 1;
            return yearCount;
        }

        private bool IsLtm(HtmlDocument document)
        {
            try
            {
                var ltm = document
                    .DocumentNode
                    .SelectNodes(".//td[class='ltm_spc']")
                    .First();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private List<object> ParseRow(HtmlDocument document, string field, Func<string, object> selector)
        {
            return document.DocumentNode
                .SelectSingleNode($".//*[@field='{field}']")
                .SelectNodes("td[not(@class)]")
                .Select(n => n.InnerText.Trim().Replace(" ", ""))
                .Select(selector)
                .ToList();
        }

        private HtmlDocument GetCachedDocument(string url)
        {
            if (cachedDocument.Item2 != null && cachedDocument.Item1 == url)
                return cachedDocument.Item2;

            cachedDocument.Item1 = url;
            cachedDocument.Item2 = web.Load(url);

            return cachedDocument.Item2;
        }

        private List<float?> SafeParseFloatRow(HtmlDocument document, string field)
        {
            try
            {
                return ParseRow(document, field, s => FloatParser.SafeParse(s))
                    .ConvertAllElements<float?>()
                    .ToList();
            }
            catch
            {
                return null;
            }
        }

        private List<float?> SafeParsePercentRow(HtmlDocument document, string field)
        {
            try
            {
                return ParseRow(document, field, s => FloatParser.SafePercentParse(s))
                .ConvertAllElements<float?>()
                .ToList();
            }
            catch
            {
                return null;
            }
        }

        private List<DateTime?> SafeParseDateTimeRow(HtmlDocument document, string field)
        {
            try
            {
                return ParseRow(document, field, s => DateTimeParser.SafeParse(s, new CultureInfo("ru-RU")))
                .ConvertAllElements<DateTime?>()
                .ToList();
            }
            catch
            {
                return null;
            }
        }

        private string GetTicker(HtmlDocument document)
        {
            var title = document.DocumentNode
                .SelectSingleNode(".//title")
                .InnerText;

            var openBracketIndex = title.IndexOf('(');
            var closeBracketIndex = title.IndexOf(')');

            return title.Substring(openBracketIndex + 1, closeBracketIndex - openBracketIndex - 1);
        }

        private string GetName(HtmlDocument document)
        {
            var title = document.DocumentNode
                .SelectSingleNode(".//title")
                .InnerText;

            return new string(title.TakeWhile(s => s != ' ').ToArray());
        }

        private List<string> GetAllFundamentalAnalysisUrls()
        {
            var document = web.Load(fundamentalAnalysisUrl);

            return document
                .DocumentNode
                .SelectNodes(".//*[@class='charticon2']")
                .Select(u =>
                {
                    if (u == null) throw new ArgumentNullException(nameof(u));
                    return u
                        .Attributes
                        .AttributesWithName("href")
                        .First()
                        .Value;
                })
                .Select(s => baseUrl + s)
                .ToList();
        }
    }
}