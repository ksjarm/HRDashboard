using employeeproject.Model;
using HRDashboardApplication.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController] [Route("api/[controller]")]
public class EmployeeShiftsController : ControllerBase {
    private readonly DataContext _context;
    public EmployeeShiftsController(DataContext context) => _context = context;

    [HttpGet] public IActionResult Get() => Ok(_context.EmployeeShiftsList);
}
