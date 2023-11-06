using Microsoft.AspNetCore.Mvc;
using WorkoutApplication.Model;
using static WorkoutApplication.Model.Employee;

namespace WorkoutApplication.Controllers;

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
        return Ok(_context.EmployeeList);
    }

    [HttpGet("{id}")]
    public IActionResult GetDetails(int? id)
    {
        var employee = _context.EmployeeList!.Find(id);
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
