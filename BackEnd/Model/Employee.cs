using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using employeeproject.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HRDashboardApplication.Model;
[Table("employee")] public record Employee {
    [Column("id")] public int Id { get; set; }
    [Column("name")] public string? Name { get; set; }
    [Column("surname")] public string? Surname { get; set; }
    [Column("gender")] public Gender? Gender { get; set; }
    [Column("dateOfBirth")] public string? DateOfBirth { get; set; }
    [Column("email")] public string? Email { get; set; }
    [Column("phonenumber")] public string? PhoneNumber { get; set; }
    [Column("adress")] public string? Adress { get; set; }
    [Column("position")] public string? Position { get; set; }
    [Column("salary")] public int? Salary { get; set; }
    [Column("status")] public Status Status { get; set; }
    [Column("photo")] public string? Photo { get; set; }
    [Column("shiftIds")] public List<int>? ShiftIds { get; set; }
    [JsonIgnore] [Column("employeeShifts")] public virtual ICollection<EmployeeShift>? EmployeeShifts { get; set; }
}
[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status {
        Active = 1,
        OnVaction = 2,
        OnMaternityLeave = 3,
        OnLeaveProcess = 4
    }
[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender {
        Male = 1,
        Female = 2,
    }

