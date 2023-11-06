using System.Text.Json.Serialization;
using HRDashboardApplication.Model;

namespace employeeproject.Model;

public record EmployeeShift {
    public int EmployeeId { get; set; }
    public int ShiftId { get; set; }
    [JsonIgnore] public virtual Employee? Employee { get; set; }
    [JsonIgnore] public virtual Shift? Shift { get; set; }
}