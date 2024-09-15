using Microsoft.AspNetCore.Mvc;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;

namespace AppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController(IAppointmentRepository appointmentRepository, ILogger logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAllAppointments()
    {
        logger.Information("Getting all appointments");
        var appointments = await appointmentRepository.GetAllAppointmentsAsync();
        logger.Information("Retrieved {Count} appointments", appointments.Count());
        return Ok(appointments);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Appointment?>> GetAppointmentById(long id)
    {
        logger.Information("Getting appointment with id: {Id}", id);
        var appointment = await appointmentRepository.GetAppointmentByIdAsync(id);
        if (appointment == null)
        {
            logger.Warning("Appointment with id: {Id} not found", id);
            return NotFound();
        }
        logger.Information("Retrieved appointment with id: {Id}", id);
        return Ok(appointment);
    }

    [HttpPost]
    public async Task<ActionResult<Appointment?>> CreateAppointment(Appointment? appointment)
    {
        logger.Information("Creating new appointment");
        var createdAppointment = await appointmentRepository.CreateAppointmentAsync(appointment);
        logger.Information("Created appointment with id: {Id}", createdAppointment.AppointmentId);
        return CreatedAtAction(nameof(GetAppointmentById), new { id = createdAppointment.AppointmentId }, createdAppointment);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAppointment(long id, Appointment appointment)
    {
        logger.Information("Updating appointment with id: {Id}", id);
        if (id != appointment.AppointmentId)
        {
            logger.Warning("Bad request: Id mismatch for appointment update");
            return BadRequest();
        }

        var updatedAppointment = await appointmentRepository.UpdateAppointmentAsync(appointment);
        if (updatedAppointment == null)
        {
            logger.Warning("Appointment with id: {Id} not found for update", id);
            return NotFound();
        }

        logger.Information("Updated appointment with id: {Id}", id);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAppointment(long id)
    {
        logger.Information("Deleting appointment with id: {Id}", id);
        await appointmentRepository.DeleteAppointmentAsync(id);
        logger.Information("Deleted appointment with id: {Id}", id);
        return NoContent();
    }
}