namespace InvestmentsApplication.ReportScraper.Extensions
{
    public static class StringExtension
    {
        public static string ReplaceEmptyOnNull(this string s)
        {
            return s == "" ? null : s;
        }
    }
}