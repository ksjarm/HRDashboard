using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace employeeproject.Model;

[Table("Shift")] public record Shift {
    [Column("Id")] public int Id { get; init; }
    [Column("Title")] public string? Title { get; init;}
    [Column("Date")] public DateTime Date { get; init;}
    [Column("StartTime")] public TimeSpan StartTime { get; init; }
    [Column("EndTime")] public TimeSpan EndTime { get; init; }
    [JsonIgnore] public virtual ICollection<EmployeeShift>? EmployeeShifts { get; set; } = new List<EmployeeShift>();
}