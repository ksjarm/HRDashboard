using employeeproject.Model;
using Microsoft.EntityFrameworkCore;
using static WorkoutApplication.Model.Exercise;

namespace WorkoutApplication.Model;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }

    public DbSet<Exercise>? ExerciseList { get; set; }
    public DbSet<Shift>? ShiftList { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresEnum<ExerciseIntensity>();

        modelBuilder.Entity<Exercise>().Property(p => p.Id).HasIdentityOptions(startValue: 4);

        modelBuilder.Entity<Exercise>().HasData(
            new Exercise
            {
                Id = 1,
                Title = "Kätekõverdused jala tõstega",
                Description = "Tavalised kätekõverdused korda mööda jalga tõstes",
                Intensity = Exercise.ExerciseIntensity.Normal,
                RecommendedDurationInSeconds = 40,
                RecommendedTimeInSecondsBeforeExercise = 10,
                RecommendedTimeInSecondsAfterExercise = 10
            },
            new Exercise
            {
                Id = 2,
                Title = "Slaalomhüpped",
                Description = "Kükist hüpped küljelt küljele",
                Intensity = Exercise.ExerciseIntensity.High,
                RecommendedDurationInSeconds = 40,
                RecommendedTimeInSecondsBeforeExercise = 10,
                RecommendedTimeInSecondsAfterExercise = 10,
                RestTimeInstructions = "Venita reie esikülge"
            },
            new Exercise
            {
                Id = 3,
                Title = "Alt läbi jooks",
                Description = "Toenglamangus jooksmine",
                Intensity = Exercise.ExerciseIntensity.Normal,
                RecommendedDurationInSeconds = 40,
                RecommendedTimeInSecondsBeforeExercise = 10,
                RecommendedTimeInSecondsAfterExercise = 10
            });
    }
}
