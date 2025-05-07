using Microsoft.AspNetCore.Mvc;

namespace K1A.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AppointmentsController : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAppointmentAsync(int id)
    {
        return Ok(id);
    }

    [HttpPost]
    public async Task<IActionResult> AddAppointmentAsync()
    {
        return Created("/api/appointments/id", null);
    }
}