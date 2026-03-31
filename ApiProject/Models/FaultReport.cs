using System;
using Swashbuckle.AspNetCore.Annotations;
namespace ApiProject.Models
{
    public class FaultReport
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Location { get; set; }

        public PriorityLevel Priority { get; set; }   // Enums.cs’den
        [SwaggerSchema(ReadOnly = true)]
        public FaultStatus Status { get; set; }       // Enums.cs’den

        public int CreatedByUserId { get; set; }
        public User? CreatedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}