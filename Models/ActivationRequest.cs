using System.ComponentModel.DataAnnotations;

namespace LionTaskManagementApp.Models
{
    public class ActivationRequest
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string UserEmail { get; set; } = string.Empty;

        [Required]
        public decimal PricePerSquareFoot { get; set; }

        [Required]
        public string RequestedRole { get; set; } = string.Empty;

        [Required]
        public bool IsApproved { get; set; }

        [Required]
        public DateTimeOffset RequestTime { get; set; } = DateTimeOffset.MinValue;

        [Required]
        public DateTimeOffset LastUpdateTime { get; set; } = DateTimeOffset.MinValue;

        [Required]
        public string DenyComents { get; set; } = string.Empty;
    }
}
