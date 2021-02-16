using Microsoft.EntityFrameworkCore;

namespace InvestmentsApplication.ReportScraper
{
    public class ReportContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Server=localhost; Port=5432; Database=Reports;User ID=postgres;Password=123123;Integrated Security=true;Pooling=true");
        }

        public DbSet<CompanyReport> CompanyReports { get; set; }
        public DbSet<YearReport> YearReports { get; set; }
    }
}