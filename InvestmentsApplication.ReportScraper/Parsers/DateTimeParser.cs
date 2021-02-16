using System;

namespace InvestmentsApplication.ReportScraper.Parsers
{
    public static class DateTimeParser
    {
        public static DateTime? SafeParse(string date, IFormatProvider provider = null)
        {
            try
            {
                return DateTime.Parse(date, provider);
            }
            catch
            {
                return null;
            }
        }
    }
}