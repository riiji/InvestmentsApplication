using System;

namespace InvestmentsApplication.ReportScraper
{
    public class YearReport
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public float? Revenue { get; set; }
        public float? NetIncome { get; set; }
        public float? CommonShare { get; set; }
        public float? NumberOfShares { get; set; }
        public float? MarketCapital { get; set; }
        public float? DividendPayout { get; set; }
        public float? Dividend { get; set; }
        public float? Opex { get; set; }
        public float? Assets { get; set; }
        public float? Eps { get; set; }
        public float? Roe { get; set; }
        public float? Roa { get; set; }
        public float? Pe { get; set; }
        public float? Ps { get; set; }
        public float? Pbv { get; set; }
        public float? Dept { get; set; }
    }
}