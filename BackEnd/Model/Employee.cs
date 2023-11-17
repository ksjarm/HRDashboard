using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using employeeproject.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HRDashboardApplication.Model;
[Table("employee")]
public record Employee
{
    [Column("id")]
    public int Id { get; init; }
    [Column("name")]
    public string? Name {get;init;}
    [Column("surname")]
    public string? Surname {get;init;}
    [Column("gender")]
    public Gender? Gender {get;init;}
    [Column("dateOfBirth")]
    public string? DateOfBirth {get;init;}
    [Column("email")]
    public string? Email {get; init;}
    [Column("phonenumber")]
    public string? PhoneNumber {get;init;}
    [Column("adress")]
    public string? Adress {get;init;}
    [Column("position")]
    public string? Position {get;init;}
     [Column("salary")]
    public int? Salary {get;init;}
    [Column("status")]
    public Status Status {get;init;}
    [Column("photo")]
    public string? Photo {get;init;}
    [JsonIgnore] public virtual ICollection<EmployeeShift>? EmployeeShifts { get; set; } = new List<EmployeeShift>();
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

