using System.Text.Json.Serialization;
using WorkoutApplication.Model;

namespace employeeproject.Model;

public record Shift {
    public int Id { get; init; }
    public string? Title { get; init;}
    public DateTime Date { get; init;}
    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
    public int? EmployeeId { get; set; }
    [JsonIgnore] public Employee? Employee { get; set; }
}