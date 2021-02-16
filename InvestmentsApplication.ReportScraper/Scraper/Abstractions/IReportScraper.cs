using System.Collections.Generic;

namespace InvestmentsApplication.ReportScraper.Scraper.Abstractions
{
    public interface IReportScraper
    {
        List<CompanyReport> ScrapeAllCompanyReports();
        CompanyReport ScrapeByUrl(string url);
    }
}