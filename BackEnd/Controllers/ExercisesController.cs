using Microsoft.AspNetCore.Mvc;
using WorkoutApplication.Model;
using static WorkoutApplication.Model.Exercise;

namespace WorkoutApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly DataContext _context;

    public ExercisesController(DataContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public IActionResult Get([FromQuery] ExerciseIntensity? intensity)
    {
        if (intensity.HasValue)
        {
            return Ok(_context.ExerciseList!.Where(x => x.Intensity == intensity.Value));
        }

        return Ok(_context.ExerciseList);
    }

    [HttpGet("{id}")]
    public IActionResult GetDetails(int? id)
    {
        var exercise = _context.ExerciseList!.Find(id);
        if (exercise == null)
        {
            return NotFound();
        }

        return Ok(exercise);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Exercise exercise)
    {
        var dbExercise = _context.ExerciseList!.Find(exercise.Id);
        if (dbExercise ==  null)
        {
            _context.Add(exercise);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetDetails), new { Id = exercise.Id }, exercise);
        }

        return Conflict();
    }

    [HttpPut("{id}")]
    public IActionResult Update(int? id, [FromBody] Exercise exercise)
    {
        if (id != exercise.Id || !_context.ExerciseList!.Any(e => e.Id == id))
        {
            return NotFound();
        }

        _context.Update(exercise);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int? id) 
    {
        var exercise = _context.ExerciseList?.Find(id);
        if (exercise == null)
        {
            return NotFound();
        }

        _context.Remove(exercise);
        _context.SaveChanges();

        return NoContent();
    }
}
