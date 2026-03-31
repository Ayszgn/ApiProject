namespace ApiProject.Models
{
    public class FaultReportCreateDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Location { get; set; }
        public PriorityLevel Priority { get; set; }
    }
}
