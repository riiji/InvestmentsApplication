using InvestmentsApplication.ReportScraper;

namespace InvestmentsApplication
{
    public interface IRealPriceAlgorithm
    {
        float? Calculate(CompanyReport report);
    }
}