using System.Text.Json.Serialization;

namespace employeeproject.Model;

public class EmployeeShift
{
    public int EmployeeId { get; set; }
    public int ShiftId { get; set; }

    //[JsonIgnore]
    //public virtual Employee? Employee { get; set; } employee is not my part of work, so i commented it to avoid the error
    [JsonIgnore]
    public virtual Shift? Shift { get; set; }
}
