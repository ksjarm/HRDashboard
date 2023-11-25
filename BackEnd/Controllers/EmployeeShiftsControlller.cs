using employeeproject.Model;
using HRDashboardApplication.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class EmployeeShiftsController : ControllerBase
{
    private readonly DataContext _context;

    public EmployeeShiftsController(DataContext context)
    {
        _context = context;
    }

    // GET: api/employeeshifts
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.EmployeeShiftsList);
    }

    [HttpGet("{id}")]
    public IActionResult GetDetails(int? id)
    {
        var employeeShift = _context.EmployeeShiftsList!.Find(id);
        if (employeeShift == null)
        {
            return NotFound();
        }

        return Ok(employeeShift);
    }

}
