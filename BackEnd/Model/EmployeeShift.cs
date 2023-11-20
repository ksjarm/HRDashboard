using System.Text.Json.Serialization;
using HRDashboardApplication.Model;

namespace employeeproject.Model;

public record EmployeeShift {
    public int EmployeeId { get; init; }
    public int ShiftId { get; init; }
    [JsonIgnore] public virtual Employee? Employee { get; init; }
    [JsonIgnore] public virtual Shift? Shift { get; init; }
}