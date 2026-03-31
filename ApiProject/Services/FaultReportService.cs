using ApiProject.Models;

namespace ApiProject.Services
{
    public class FaultReportService
    {
        public void UpdatePriority(FaultReport report, PriorityLevel newPriority)
        {
            if (report.Priority == PriorityLevel.High && newPriority == PriorityLevel.Low)
            {
                throw new Exception("High priority düşürülemez!");
            }
            report.Priority = newPriority;
        }
    }
}
