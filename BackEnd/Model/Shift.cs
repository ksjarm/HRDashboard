using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace employeeproject.Model;

[Table("Shift")] public record Shift {
    [Column("id")] public int Id { get; init; }
    [Column("Title")] public string? Title { get; init;}
    [Column("Date")] public string? Date { get; init;}
    [Column("StartTime")] public string? StartTime { get; init; }
    [Column("EndTime")] public string? EndTime { get; init; }
   // [JsonIgnore] public virtual ICollection<EmployeeShift>? EmployeeShifts { get; set; } = new List<EmployeeShift>();
}