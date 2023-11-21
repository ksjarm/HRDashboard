using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using employeeproject.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HRDashboardApplication.Model;

public class Employee
{[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
   
    public int Id { get; set; }
  
    public string? Name {get;set;}
   
    public string? Surname {get;set;}
   
    public Gender? Gender {get;set;}
  
    public string? DateOfBirth {get;set;}
    
    public string? Email {get; set;}

    public string? PhoneNumber {get;set;}
    
    public string? Adress {get;set;}
    
    public string? Position {get;set;}
    
    public int? Salary {get;set;}
   
    public Status Status {get;set;}
    [JsonIgnore] public virtual ICollection<EmployeeShift> EmployeeShifts { get; set; }=new List<EmployeeShift>();
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

