using System;
using System.Linq;
using InvestmentsApplication.ReportScraper;

namespace InvestmentsApplication
{
    public class SimpleRealPriceAlgorithm : IRealPriceAlgorithm
    {
        private float epsGrowthError = 0.30f;

        public float? Calculate(CompanyReport report)
        {
            try
            {
                if (!IsPositiveNetIncome(report) || !IsPositiveFlow(report) || !IsNormalEps(report))
                    return null;

                int yearsCount = report.YearReports.Count;
                var epsGrowth = Math.Pow((double)report.YearReports.Last().Eps / (double)report.YearReports.First().Eps, 1.0d / (yearsCount-1+yearsCount*epsGrowthError));

                if (epsGrowth < 1)
                    return null;

                var averagePe = report.YearReports.Select(n => n.Pe).Sum() / yearsCount;
                var price = report.YearReports.Last().Eps * epsGrowth * averagePe;

                return (float?)price;
            }
            catch
            {
                return null;
            }
        }

        public float? CalculateInterest(float calculatedPrice, float realPrice)
        {
            if (realPrice > calculatedPrice)
                return null;

            return calculatedPrice / realPrice; 
        }

        private bool IsNormalEps(CompanyReport report)
        {
            if (report.YearReports.All(s => s.Eps > 1))
                return true;
            return false;
        }

        private bool IsPositiveNetIncome(CompanyReport report)
        {
            return report.YearReports
                .Select(r => r.NetIncome)
                .Sum() > 0.0f;
        }

        private bool IsPositiveFlow(CompanyReport report)
        {
            int positiveFlowYearCount = 0;
            int negativeFlowYearCount = 0;

            for (int i = 0; i < report.YearReports.Count - 1; i++)
            {
                if (report.YearReports[i + 1].NetIncome > report.YearReports[i].NetIncome)
                {
                    positiveFlowYearCount++;
                }
                else
                {
                    negativeFlowYearCount++;
                }
            }

            return positiveFlowYearCount > negativeFlowYearCount;
        }
    }
}