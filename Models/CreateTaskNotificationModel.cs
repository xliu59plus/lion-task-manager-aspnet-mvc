namespace LionTaskManagementApp.Models
{
    public class CreateTaskNotificationModel
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public String? UserId { get; set; }
        public bool isNotified { get; set; }
        public double distance { get; set; }
    }
}
