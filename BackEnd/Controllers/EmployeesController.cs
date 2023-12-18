using Microsoft.AspNetCore.Mvc;
using HRDashboardApplication.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using employeeproject.Model;

namespace HRDashboardApplication.Controllers;

 [ApiController] [Route("api/[controller]")] public class EmployeesController : ControllerBase {
    private readonly DataContext _context;
    public EmployeesController(DataContext context) => _context = context;

    [HttpGet] public IActionResult Get() {
        var employeesWithShifts = _context.EmployeeList!.Include(e => e.EmployeeShifts)!.ThenInclude(es => es.Shift).ToList();
        return Ok(employeesWithShifts);
    }
    [HttpGet("{id}")] public IActionResult GetDetails(int? id) {
        if (_context.EmployeeList!.Find(id) == null) return NotFound();
        var employee = _context.EmployeeList!.Include(e => e.EmployeeShifts)!.ThenInclude(es => es.Shift).FirstOrDefault(e => e.Id == id);
        return Ok(employee);
    }
    [HttpPost] public IActionResult Create([FromBody] Employee employee) {
        var dbEmployee = _context.EmployeeList!.Find(employee.Id);
        if (dbEmployee !=  null) return Conflict("Employee with given Id already exists.");
        employee.Email = employee.Name + "." + employee.Surname + "@" + "company.ee";

        if(employee.Position == null || employee.Position == ""){
            return BadRequest();
        }

        if (employee.ShiftIds != null && employee.ShiftIds[0] != 0) {
            var employeeShifts = new List<EmployeeShift>();
            foreach (var shiftId in employee.ShiftIds) {
                if (_context.EmployeeList!.Find(shiftId) == null) return NotFound($"Shift with ID {shiftId} not found.");
                var newEmployeeShift = new EmployeeShift {
                    EmployeeId = employee.Id,
                    ShiftId = shiftId
                };
                employeeShifts.Add(newEmployeeShift);
            }
            employee.EmployeeShifts = employeeShifts;
        }

        _context.Add(employee);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetDetails), new { id = employee.Id }, employee);
    }
    [HttpPut("{id}")] public IActionResult Update(int? id, [FromBody] Employee employee) {
        if (id != employee.Id) return BadRequest("The ID in the URL does not match the ID in the request body.");
        var existingEmployee = _context.EmployeeList!.Include(e => e.EmployeeShifts).FirstOrDefault(e => e.Id == id);
        if (existingEmployee == null) return NotFound("Employee with given Id not found.");

        existingEmployee.Name = employee.Name;
        existingEmployee.Surname = employee.Surname;
        existingEmployee.Gender = employee.Gender;
        existingEmployee.DateOfBirth = employee.DateOfBirth;
        existingEmployee.Email = employee.Email;
        existingEmployee.PhoneNumber = employee.PhoneNumber;
        existingEmployee.Adress = employee.Adress;
        existingEmployee.Position = employee.Position;
        existingEmployee.Salary = employee.Salary;
        existingEmployee.Status = employee.Status;
        existingEmployee.Photo = employee.Photo;
        existingEmployee.ShiftIds = employee.ShiftIds;

        if (employee.ShiftIds != null && employee.ShiftIds[0] != 0) {
            existingEmployee.EmployeeShifts!.Clear();
            foreach (var shiftId in employee.ShiftIds) {
                var shift = _context.EmployeeList!.Find(shiftId);
                if (shift == null) return NotFound($"Shift with ID {shiftId} not found.");
                var newEmployeeShift = new EmployeeShift {
                    EmployeeId = employee.Id,
                    ShiftId = shiftId
                };
                existingEmployee.EmployeeShifts.Add(newEmployeeShift);
            }
        }
        else existingEmployee.EmployeeShifts!.Clear();

        _context.SaveChanges();
        return NoContent();
    }
    [HttpDelete("{id}")] public IActionResult Delete(int? id) {
        var employee = _context.EmployeeList?.Find(id);
        if (employee == null) return NotFound("Employee with given Id not found.");
        _context.Remove(employee);
        _context.SaveChanges();
        return NoContent();
    }
    [HttpGet("currentEmployeeCount")] public IActionResult GetCurrentEmployeeCount() {
        int currentEmployeeCount = _context.EmployeeList!.Count();
        return Ok(currentEmployeeCount);
    }
    [HttpGet("{id}/shifts")] public IActionResult GetEmployeeShifts(int? id) {
        var employee = _context.EmployeeList!.Include(e => e.EmployeeShifts).FirstOrDefault(e => e.Id == id);
        if (employee == null) return NotFound($"Employee with ID {id} not found.");
        var employeeShifts = employee.EmployeeShifts;
        return Ok(employeeShifts);
    }    
}