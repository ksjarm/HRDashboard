using System.Text.Json.Serialization;

namespace employeeproject.Model;

public record Shift
{
    public int Id { get; init; }
    public string? Title {get; init;}
    public DateTime Date {get; init;}
    public DateTime StartTime {get; init; }
    public DateTime EndTime {get; init; }

    [JsonIgnore]
    public virtual EmployeeShift? EmployeeShifts { get; set; }
}