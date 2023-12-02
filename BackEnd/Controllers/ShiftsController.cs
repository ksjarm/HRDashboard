using employeeproject.Model;
using HRDashboardApplication.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers;

[ApiController] [Route("api/[controller]")] public class ShiftsController : ControllerBase {
    private readonly DataContext _context;
    public ShiftsController(DataContext context) => _context = context;   

    [HttpGet] public IActionResult Get() {
        var shiftsWithEmployees = _context.ShiftList!.Include(s => s.EmployeeShifts)!.ThenInclude(es => es.Employee).ToList();
        return Ok(shiftsWithEmployees);
    }
    [HttpGet("{id}")] public IActionResult GetDetails(int? id) {
        if (_context.ShiftList!.Find(id) == null) return NotFound("Shift with given Id not found.");
        var shift = _context.ShiftList!.Include(s => s.EmployeeShifts)!.ThenInclude(es => es.Employee).FirstOrDefault(s => s.Id == id);
        return Ok(shift);
    }
    [HttpPost] public IActionResult Create([FromBody] Shift shift) {
        var dbShift = _context.ShiftList!.Find(shift.Id);
        if (dbShift !=  null) return Conflict("Shift with given Id already exists.");
    
        if (shift.EmployeeIds != null) {
            var employeeShifts = new List<EmployeeShift>();
            foreach (var employeeId in shift.EmployeeIds) {
                if (_context.EmployeeList!.Find(employeeId) == null) return NotFound($"Employee with ID {employeeId} not found.");
                var newEmployeeShift = new EmployeeShift {
                    EmployeeId = employeeId,
                    ShiftId = shift.Id
                };
                employeeShifts.Add(newEmployeeShift);
            }
            shift.EmployeeShifts = employeeShifts;
        }

        _context.Add(shift);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetDetails), new { id = shift.Id }, shift);
    }
    [HttpPut("{id}")] public IActionResult Update(int? id, [FromBody] Shift shift) {
        if (id != shift.Id) return BadRequest("The ID in the URL does not match the ID in the request body.");
        var existingShift = _context.ShiftList!.Include(s => s.EmployeeShifts).FirstOrDefault(s => s.Id == id);
        if (existingShift == null) return NotFound("Shift with given Id not found.");

        existingShift.Title = shift.Title;
        existingShift.Date = shift.Date;
        existingShift.StartTime = shift.StartTime;
        existingShift.EndTime = shift.EndTime;
        existingShift.Valik = shift.Valik;
        existingShift.StartDate = shift.StartDate;
        existingShift.EndDate = shift.EndDate;
        existingShift.SelectedWeekDay = shift.SelectedWeekDay;
        existingShift.EmployeeIds = shift.EmployeeIds;

        if (shift.EmployeeIds != null) {
            existingShift.EmployeeShifts!.Clear();
            foreach (var employeeId in shift.EmployeeIds) {
                var employee = _context.EmployeeList!.Find(employeeId);
                if (employee == null) return NotFound($"Employee with ID {employeeId} not found.");
                var newEmployeeShift = new EmployeeShift {
                    EmployeeId = employeeId,
                    ShiftId = existingShift.Id
                };
                existingShift.EmployeeShifts.Add(newEmployeeShift);
            }
        }
        else existingShift.EmployeeShifts!.Clear();

        _context.SaveChanges();
        return NoContent();
    }
    [HttpDelete("{id}")] public IActionResult Delete(int? id) {
        var shift = _context.ShiftList?.Find(id);
        if (shift == null) return NotFound("Shift with given Id not found.");
        _context.Remove(shift);
        _context.SaveChanges();
        return NoContent();
    }
}