using ApiProject.Models;

namespace ApiProject.Services
{
    public static class FaultStatusHelper
    {
        public static readonly Dictionary<FaultStatus, FaultStatus[]> StatusTransitions = new()
    {
        { FaultStatus.New,       new[] { FaultStatus.InReview, FaultStatus.Cancelled } },
        { FaultStatus.InReview,  new[] { FaultStatus.Assigned, FaultStatus.Rejected, FaultStatus.Cancelled } },
        { FaultStatus.Assigned,  new[] { FaultStatus.Working, FaultStatus.Cancelled } },
        { FaultStatus.Working,   new[] { FaultStatus.Completed, FaultStatus.Cancelled } },
        { FaultStatus.Completed, Array.Empty<FaultStatus>() },
        { FaultStatus.Cancelled, Array.Empty<FaultStatus>() },
        { FaultStatus.Rejected,  Array.Empty<FaultStatus>() }
    };
    }
}
