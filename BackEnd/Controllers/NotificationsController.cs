using BackEnd.Model;
using Microsoft.AspNetCore.Mvc;
using HRDashboardApplication.Model;

namespace BackEnd.Controllers;

[ApiController] [Route("api/[controller]")]
public class NotificationsController : ControllerBase {
    private readonly DataContext _context;
    public NotificationsController(DataContext context)=> _context = context;

    [HttpGet] public IActionResult GetNotifications() => Ok(_context.NotificationsList);
    [HttpGet("{id}")] public IActionResult GetDetails(int? id) {
        var notification = _context.NotificationsList!.Find(id);
        if (notification == null) return NotFound();
        return Ok(notification);
    }
    [HttpPost] public IActionResult AddNotification([FromBody] Notification notification) {
        var dbNotification = _context.ShiftList!.Find(notification.NotificationId);
        if (dbNotification ==  null) {
            _context.NotificationsList.Add(notification);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetDetails), new { id = notification.NotificationId }, notification);
        }
        return Conflict();
    }
    [HttpPut("{id}")]public IActionResult UpdateNotification(int? id, [FromBody]  Notification notification)
    {
        if (id != notification.NotificationId || !_context.NotificationsList!.Any(e => e.NotificationId == id)) return NotFound();
        _context.Update(notification);
        _context.SaveChanges();
        return NoContent();
    }
    [HttpDelete("{id}")] public IActionResult DeleteNotification(int id) {
        var notification = _context.NotificationsList.Find(id);
        if (notification == null) return NotFound();
        _context.NotificationsList.Remove(notification);
        _context.SaveChanges();

        return NoContent();
    }
    [HttpDelete("deleteAll")] public IActionResult DeleteAllNotifications() {
        _context.NotificationsList.RemoveRange(_context.NotificationsList);
        _context.SaveChanges();
        return NoContent();
    }
}
