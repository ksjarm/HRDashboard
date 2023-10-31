using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Model
{
    public record Notification {
        public int NotificationId { get; set; } 
        public string? Message { get; set; }
        public DateTime Date { get; set; } 
        public string? Type { get; set; } 
    }
}