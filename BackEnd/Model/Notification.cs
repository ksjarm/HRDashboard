using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Model
{
    public record Notification {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; } 
        public string? Message { get; set; }
        public DateTime Date { get; set; } 
        public string? Type { get; set; } 
    }
}