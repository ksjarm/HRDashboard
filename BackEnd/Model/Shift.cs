using System.Text.Json.Serialization;

namespace employeeproject.Model;

public record Shift {
    public int Id { get; init; }
    public string? Title { get; init;}
    public DateTime Date { get; init;}
    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
    [JsonIgnore] public virtual EmployeeShift? EmployeeShifts { get; set; }
}