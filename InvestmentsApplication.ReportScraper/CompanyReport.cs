using System.Collections.Generic;

namespace InvestmentsApplication.ReportScraper
{
    public class CompanyReport
    {
        public int Id { get; set; }
        public string Ticker { get; set; }
        public string Name { get; set; }
        public List<YearReport> YearReports { get; set; }
        public YearReport LtmReport { get; set; }
    }
}