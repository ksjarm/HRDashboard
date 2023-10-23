using employeeproject.Model;
using WorkoutApplication.Model;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers;

[ApiController] [Route("api/[controller]")]
public class ShiftsController : ControllerBase {
    private readonly DataContext _context;
    public ShiftsController(DataContext context) => _context = context;   

    [HttpGet] public IActionResult Get() => Ok(_context.ShiftList);
    [HttpGet("{id}")] public IActionResult GetDetails(int? id) {
        var shift = _context.ShiftList!.Find(id);
        if (shift == null) return NotFound();
        return Ok(shift);
    }
    [HttpPost] public IActionResult Create([FromBody] Shift shift) {
        var dbShift = _context.ShiftList!.Find(shift.Id);
        if (dbShift ==  null) {
            _context.Add(shift);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetDetails), new { Id = shift.Id }, shift);
        }
        return Conflict();
    }
    [HttpPut("{id}")] public IActionResult Update(int? id, [FromBody] Shift shift) {
        if (id != shift.Id || !_context.ShiftList!.Any(e => e.Id == id)) return NotFound();
        _context.Update(shift);
        _context.SaveChanges();
        return NoContent();
    }
    [HttpDelete("{id}")] public IActionResult Delete(int? id) {
        var shift = _context.ShiftList?.Find(id);
        if (shift == null) return NotFound();
        _context.Remove(shift);
        _context.SaveChanges();
        return NoContent();
    }
}