using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace employeeproject.Model;

public class Shift {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
     public string? Title { get; set;}
    public string? Date { get; set;}
    public string? StartTime { get; set; }
     public string? EndTime { get; set; }
     public Valik Valik { get; set; }
       public string? StartDate { get; set; }
   public string? EndDate { get; set; }
     public string? SelectedWeekDay { get; set; }
     
     [JsonIgnore] public virtual ICollection<EmployeeShift> EmployeeShift { get; set; }=new List<EmployeeShift>();
      [NotMapped]
        public IEnumerable<string?> AssignedEmployeesNames
        {
            get
            {
                return EmployeeShift.Select(es => es.Employee?.Name).Where(name => name != null);
            }
        }

}
[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Valik {
        Onetime=1,
        Recurring=2

    }