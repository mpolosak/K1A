using K1A.DTOs;
using K1A.Exceptions;
using K1A.Service;
using Microsoft.AspNetCore.Mvc;

namespace K1A.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AppointmentsController(IAppointmentsService _service) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAppointmentAsync(int id)
    {
        try
        {
            var appointment = await _service.GetAppointmentAsync(id);
            return Ok(appointment);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddAppointmentAsync([FromBody] PushAppointmentDTO appointment)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            return Created("/api/appointments/id", "created new appointment");
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }
}