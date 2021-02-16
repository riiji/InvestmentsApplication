using System.Collections.Generic;
using System.Linq;

namespace InvestmentsApplication.ReportScraper.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ConvertAllElements<T>(this IEnumerable<object> enumerable)
        {
            return enumerable
                .Select(s => (T)s);
        }
    }
}