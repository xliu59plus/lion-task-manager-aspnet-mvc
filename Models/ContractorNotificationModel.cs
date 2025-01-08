using System.Reflection;

namespace LionTaskManagementApp.Models
{
    public class ContractorNotificationModel
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public String? NotifiedUserId { get; set; }
        public bool IsNotified { get; set; }
        public bool IsRead { get; set; }
        public double Distance { get; set; }
        public String Title { get; set; } = String.Empty;
        public decimal Budget { get; set; }
        public DateTimeOffset CreatedDatetime { get; set; }
        public DateTimeOffset LastUpdatedDatetime { get; set; }
        public DateTimeOffset ExpirationDatetime { get; set; }

        public string DistanceInMiles => (Distance * 0.621371).ToString("F2");
    }
}