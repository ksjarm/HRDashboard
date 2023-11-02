using System.Text.Json.Serialization;
using WorkoutApplication.Model;

namespace employeeproject.Model;

public class EmployeeShift {
    public int EmployeeId { get; set; }
    public int ShiftId { get; set; }

    [JsonIgnore]
    public virtual Employee? Employee { get; set; }
    [JsonIgnore]
    public virtual Shift? Shift { get; set; }
}