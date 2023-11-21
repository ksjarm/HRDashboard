using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace employeeproject.Model;

[Table("Shift")] public record Shift {
    [Column("id")] public int Id { get; init; }
    [Column("title")] public string? Title { get; init;}
    [Column("date")] public string? Date { get; init;}
    [Column("startTime")] public string? StartTime { get; init; }
    [Column("endTime")] public string? EndTime { get; init; }
     [Column("valik")] public Valik Valik { get; init; }
      [Column("startDate")] public string? StartDate { get; init; }
    [Column("endDate")] public string? EndDate { get; init; }
     [Column("selectedWeekDay")] public string? SelectedWeekDay { get; init; }
    [JsonIgnore] [Column("Employees")] public ICollection<EmployeeShift>? Employees { get; init; }
   

}
[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Valik {
        Onetime=1,
        Recurring=2

    }