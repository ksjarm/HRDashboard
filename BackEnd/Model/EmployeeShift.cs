using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using HRDashboardApplication.Model;

namespace employeeproject.Model;

[Table("employeeShift")] public class EmployeeShift {
    [Column("employeeId")] public int EmployeeId { get; set; }
    [Column("shiftId")] public int ShiftId { get; set; }
    [JsonIgnore] [Column("employee")] public virtual Employee? Employee { get; set; }
    [JsonIgnore] [Column("shift")] public virtual Shift? Shift { get; set; }
}