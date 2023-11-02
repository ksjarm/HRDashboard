using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using employeeproject.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WorkoutApplication.Model;
[Table("employee")]
public record Employee
{
    [Column("id")]
    public int Id { get; init; }
    [Column("Name")]
    public string? Name {get;init;}
    [Column("Surname")]
    public string? Surname {get;init;}
    [Column("Gender")]
    public Gender? Gender {get;init;}
    [Column("DateOfBirth")]
    public string? DateOfBirth {get;init;}
    [Column("Email")]
    public string? Email {get; init;}
    [Column("Phonenumber")]
    public string? PhoneNumber {get;init;}
    [Column("Adress")]
    public string? Adress {get;init;}
    [Column("Position")]
    public string? Position {get;init;}
     [Column("Salary")]
    public int? Salary {get;init;}
    [Column("Status")]
    public Status Status {get;init;}
    public int? ShiftId { get; set; }
    [JsonIgnore] public Shift? Shift { get; set; }
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

