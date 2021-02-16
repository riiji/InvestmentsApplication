namespace InvestmentsApplication.ReportScraper
{
    public class ReportRepository : IReportRepository
    {
        private readonly ReportContext context;

        public ReportRepository(ReportContext context)
        {
            this.context = context;
        }

        public void AddCompanyReport(CompanyReport report)
        {
            context.Add(report);
            context.SaveChanges();
        }

        public void UpdateCompanyReport(CompanyReport report)
        {
            context.Update(report);
            context.SaveChanges();
        }
    }
}