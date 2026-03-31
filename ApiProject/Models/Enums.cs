namespace ApiProject.Models
{
    public enum PriorityLevel
    {
        Low,
        Medium,
        High
    }
    public enum FaultStatus
    {
        New,         //0
        InReview,    //1
        Assigned,    //2
        Working,     //3
        Completed,   //4
        Cancelled,   //5
        Rejected     //6
    }
}
