namespace InvestmentsApplication.ReportScraper
{
    public interface IReportRepository
    {
        void AddCompanyReport(CompanyReport report);
        void UpdateCompanyReport(CompanyReport report);
    }
}