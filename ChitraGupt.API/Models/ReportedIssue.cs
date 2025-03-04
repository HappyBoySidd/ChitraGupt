namespace Chitragupt_API.Models
{
    public class ReportedIssue
    {
        public long DefectId { get; set; }
        public required string Description { get; set; }
        public string? ShortDescription { get; set; }
        public string? L0 { get; set; }
        public string? L1 { get; set; }
        public string? L2 { get; set; }
        public string? L3 { get; set; }
    }
}
