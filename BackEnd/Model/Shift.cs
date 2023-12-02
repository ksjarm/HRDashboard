using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace employeeproject.Model;

[Table("shift")] public class Shift {
    [Column("id")] public int Id { get; set; }
    [Column("title")] public string? Title { get; set;}
    [Column("date")] public string? Date { get; set;}
    [Column("startTime")] public string? StartTime { get; set; }
    [Column("endTime")] public string? EndTime { get; set; }
    [Column("valik")] public Valik Valik { get; set; }
    [Column("startDate")] public string? StartDate { get; set; }
    [Column("endDate")] public string? EndDate { get; set; }
    [Column("selectedWeekDay")] public string? SelectedWeekDay { get; set; }
    [Column("employeeIds")] public List<int>? EmployeeIds { get; set; }
    [JsonIgnore] [Column("employeeShifts")] public virtual ICollection<EmployeeShift>? EmployeeShifts { get; set; }
}
[JsonConverter(typeof(JsonStringEnumConverter))] public enum Valik {
    Onetime=1,
    Recurring=2
}