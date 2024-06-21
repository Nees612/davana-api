namespace API.Services.SimpleNotification.Models
{
    public class Message
    {
        public string? MarketId { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public bool IsInfo { get; set; }
        public string? MessageGroupId { get; set; }
        public string? deduplicationId { get; set; }
        public Dictionary<string, string>? MessageAttributes { get; set; }
    }
}