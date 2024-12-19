namespace LionTaskManagementApp.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public bool IsRead { get; set; }
    }
}
