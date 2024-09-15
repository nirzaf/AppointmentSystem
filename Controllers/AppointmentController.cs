using Microsoft.AspNetCore.Mvc;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;
using Microsoft.Extensions.Logging;

namespace AppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController(IAppointmentRepository appointmentRepository, ILogger<AppointmentController> logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAllAppointments()
    {
        logger.LogInformation("Getting all appointments");
        var appointments = await appointmentRepository.GetAllAppointmentsAsync();
        logger.LogInformation("Retrieved {Count} appointments", appointments.Count());
        return Ok(appointments);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Appointment?>> GetAppointmentById(long id)
    {
        logger.LogInformation("Getting appointment with id: {Id}", id);
        var appointment = await appointmentRepository.GetAppointmentByIdAsync(id);
        if (appointment == null)
        {
            logger.LogWarning("Appointment with id: {Id} not found", id);
            return NotFound();
        }
        logger.LogInformation("Retrieved appointment with id: {Id}", id);
        return Ok(appointment);
    }

    [HttpPost]
    public async Task<ActionResult<Appointment?>> CreateAppointment(Appointment? appointment)
    {
        logger.LogInformation("Creating new appointment");
        var createdAppointment = await appointmentRepository.CreateAppointmentAsync(appointment);
        logger.LogInformation("Created appointment with id: {Id}", createdAppointment?.AppointmentId);
        return CreatedAtAction(nameof(GetAppointmentById), new { id = createdAppointment?.AppointmentId }, createdAppointment);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAppointment(long id, Appointment appointment)
    {
        logger.LogInformation("Updating appointment with id: {Id}", id);
        if (id != appointment.AppointmentId)
        {
            logger.LogWarning("Bad request: Id mismatch for appointment update");
            return BadRequest();
        }

        var updatedAppointment = await appointmentRepository.UpdateAppointmentAsync(appointment);
        if (updatedAppointment == null)
        {
            logger.LogWarning("Appointment with id: {Id} not found for update", id);
            return NotFound();
        }

        logger.LogInformation("Updated appointment with id: {Id}", id);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAppointment(long id)
    {
        logger.LogInformation("Deleting appointment with id: {Id}", id);
        await appointmentRepository.DeleteAppointmentAsync(id);
        logger.LogInformation("Deleted appointment with id: {Id}", id);
        return NoContent();
    }
}