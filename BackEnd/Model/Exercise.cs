using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WorkoutApplication.Model;
[Table("exercise")]

public record Exercise
{
    [Column("id")]
    public int Id { get; init; }
    [Column("title")]
    public string? Title {get; init;}
    [Column("description")]
    public string? Description {get; init;}
    [Column("intensity")]
    public ExerciseIntensity Intensity {get; init; }
    [Column("recommended_duration_in_seconds")]
    public int RecommendedDurationInSeconds {get; init; }
    [Column("recommended_time_in_seconds_before_exercise")]
    public int RecommendedTimeInSecondsBeforeExercise { get; init; }
    [Column("recommended_time_in_seconds_after_exercise")]
    public int RecommendedTimeInSecondsAfterExercise { get; init; }
    [Column("rest_time_instructions")]
    public string? RestTimeInstructions {get; init; }
    public string? Name {get;set;}
    public string? Surname {get;set;}
    public string? Email {get; set;}
    public string? PhoneNumber {get;set;}
    public string? Adress {get;set;}
    public int? Salary {get;set;}
    public Status Status {get;set;}

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ExerciseIntensity
    {
        Low = 1,
        Normal = 2,
        High = 3,
    }
}
[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status {
        Active = 1,
        OnVaction = 2,
        OnMaternityLeave = 3,
        OnLeaveProcess = 4
    }

