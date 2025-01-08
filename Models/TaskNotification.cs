namespace LionTaskManagementApp.Models
{
    public class TaskNotification
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = string.Empty;
        public string RecipientId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int TaskId { get; set; }
        public double Distance { get; set; } 
        public DateTimeOffset MessageTimestamp { get; set; }
        public bool IsRead { get; set; }
    }
}
