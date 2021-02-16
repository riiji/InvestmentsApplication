using System;

namespace InvestmentsApplication.ReportScraper.Parsers
{
    public static class FloatParser
    {
        public static float? SafeParse(string value)
        {
            try
            {
                return float.Parse(value);
            }
            catch
            {
                return null;
            }
        }

        public static float? SafePercentParse(string value)
        {
            try
            {
                value = value.Replace("%", "");
                var parsedValue = SafeParse(value);
                return parsedValue / 100.0f;
            }
            catch
            {
                return null;
            }
        }
    }
}