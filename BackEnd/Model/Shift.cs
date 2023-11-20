using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace employeeproject.Model;

[Table("Shift")] public record Shift {
    [Column("id")] public int Id { get; init; }
    [Column("Title")] public string? Title { get; init;}
    [Column("Date")] public string? Date { get; init;}
    [Column("StartTime")] public string? StartTime { get; init; }
    [Column("EndTime")] public string? EndTime { get; init; }
     [Column("Valik")] public Valik Valik { get; init; }
      [Column("StartDate")] public string? StartDate { get; init; }
    [Column("EndDate")] public string? EndDate { get; init; }
     [Column("SelectedWeekDay")] public string? SelectedWeekDay { get; init; }
    [JsonIgnore] [Column("Employees")] public ICollection<EmployeeShift>? Employees { get; init; }
   

}
[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Valik {
        Onetime=1,
        Recurring=2

    }