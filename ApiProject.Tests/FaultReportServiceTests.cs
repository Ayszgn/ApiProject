using Xunit;
using ApiProject.Models;
using ApiProject.Services;

namespace ApiProject.Tests
{
    public class FaultReportServiceTests
    {
        [Fact]
        public void UpdatePriority_ChangesPriority_WhenValid()
        {
            var service = new FaultReportService();

            var report = new FaultReport
            {
                Title = "Test Başlık",
                Description = "Test Açıklama",
                Location = "Test Lokasyon",
                Priority = PriorityLevel.Low
            };

            service.UpdatePriority(report, PriorityLevel.High);

            Assert.Equal(PriorityLevel.High, report.Priority);
        }
        [Fact]
        public void UpdatePriority_ShouldThrowException_WhenHighToLow()
        {
            var service = new FaultReportService();
            var report = new FaultReport
            {
                Title = "Test Başlık",
                Description = "Test Açıklama",
                Location = "Test Lokasyon",
                Priority = PriorityLevel.High
            };
            Assert.Throws<Exception>(() =>
            service.UpdatePriority(report, PriorityLevel.Low));
        }
    }
}