using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using HRDashboardApplication.Model;

namespace employeeproject.Model;

public class EmployeeShift {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     public int EmployeeId { get; set; }
     public int ShiftId { get; set; }

    [JsonIgnore] public virtual Employee? Employee { get; set; }
    [JsonIgnore]  public virtual Shift? Shift { get; set; }
}