using Microsoft.AspNetCore.Mvc;
using HRDashboardApplication.Model;
using static HRDashboardApplication.Model.Employee;
using Microsoft.EntityFrameworkCore;

namespace HRDashboardApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly DataContext _context;

    public EmployeesController(DataContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        var employees = _context.EmployeeList;
        if (employees!.Include(s => s.Shifts) != null) {
            var employeesWithShifts = employees!.Include(s => s.Shifts)!.ThenInclude(es => es.Shift).ToList();
            return Ok(employeesWithShifts);
        }
        else return Ok(employees!.Include(s => s.Shifts).ToList());    }

    [HttpGet("{id}")]
    public IActionResult GetDetails(int? id)
    {
        var employee = _context.EmployeeList!.Include(s => s.Shifts)!.ThenInclude(es => es.Shift).FirstOrDefault(s => s.Id == id);
        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Employee employee)
    {
        var dbEmployee = _context.EmployeeList!.Find(employee.Id);
        if (dbEmployee ==  null)
        {
            _context.Add(employee);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetDetails), new { Id = employee.Id }, employee);
        }

        return Conflict();
    }

    [HttpPut("{id}")]
    public IActionResult Update(int? id, [FromBody] Employee employee)
    {
        if (id != employee.Id || !_context.EmployeeList!.Any(e => e.Id == id))
        {
            return NotFound();
        }

        _context.Update(employee);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int? id) 
    {
        var employee = _context.
        EmployeeList?.Find(id);
        if (employee == null)
        {
            return NotFound();
        }

        _context.Remove(employee);
        _context.SaveChanges();

        return NoContent();
    }
    [HttpGet("currentEmployeeCount")]
    public IActionResult GetCurrentEmployeeCount()
    {
        int currentEmployeeCount = _context.EmployeeList!.Count();
        return Ok(currentEmployeeCount);
    }

}
