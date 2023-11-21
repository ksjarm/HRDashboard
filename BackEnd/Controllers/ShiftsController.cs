using employeeproject.Model;
using HRDashboardApplication.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers;

[ApiController] [Route("api/[controller]")]
public class ShiftsController : ControllerBase {
    private readonly DataContext _context;
    public ShiftsController(DataContext context) => _context = context;   

     [HttpGet]
    public IActionResult Get()
    {
        var shifts = _context.ShiftList!
            .Include(s => s.EmployeeShift)
            .ThenInclude(es => es.Employee)
            .ToList();

        // Convert the shifts to a DTO or an anonymous type to include AssignedEmployeesNames
        var shiftsResponse = shifts.Select(shift => new
        {
            shift.Id,
            shift.Title,
            shift.Date,
            shift.StartTime,
            shift.EndTime,
            shift.Valik,
            shift.StartDate,
            shift.EndDate,
            shift.SelectedWeekDay,
            AssignedEmployeesNames = shift.AssignedEmployeesNames.ToList()
        });

        return Ok(shiftsResponse);
    }
   [HttpGet("{id}")]
    public IActionResult GetDetails(int? id)
    {
        var shift = _context.ShiftList!
            .Include(s => s.EmployeeShift)
            .ThenInclude(es => es.Employee)
            .FirstOrDefault(s => s.Id == id);

        if (shift == null)
            return NotFound();

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
        if (id != shift.Id) return BadRequest("Invalid request. The ID in the URL does not match the ID in the request body.");
        var existingShift = _context.ShiftList!.Find(id);
        if (existingShift == null) return NotFound("Shift not found.");
        if (id != shift.Id || !_context.ShiftList!.Any(e => e.Id == id)) return NotFound();
        _context.Entry(existingShift).State = EntityState.Detached;
        _context.Update(shift);
        _context.SaveChanges();
        return NoContent();
    }
    [HttpDelete("{id}")] public IActionResult Delete(int? id) {
        var shift = _context.ShiftList?.Find(id);
        if (shift == null) return NotFound(); // Return a 404 Not Found if the shift doesn't exist.
        _context.Remove(shift);
        _context.SaveChanges();
        return NoContent();
    }
}