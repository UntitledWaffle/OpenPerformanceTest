namespace Events.Models
{
    public class Event
    {
        public int? EventID { get; set; }
        public string? Title { get; set; }
        public string? Location { get; set; }
        public string? Status { get; set; }
        public DateTime? Date { get; set; }
    }
}