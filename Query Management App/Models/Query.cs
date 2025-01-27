namespace Query_Management_App.Models
{
    public class Query
    {
        public int Id { get; set; }
        public string Requestor { get; set; }
        public string Status { get; set; } // Raised, OnHold, Resolved, Cancelled
        public DateTime CreatedAt { get; set; }
        public DateTime? OnHoldUntil { get; set; }
    }
}
