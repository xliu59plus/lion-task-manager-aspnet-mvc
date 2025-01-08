namespace LionTaskManagementApp.Models
{
    public class CreateTaskNotificationQueueModel
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public String? NotifiedUserId { get; set; }
        public bool IsNotified { get; set; }
        public double Distance { get; set; }
    }
}
