using Microsoft.AspNetCore.Mvc;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;

namespace AppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController(IAppointmentRepository appointmentRepository, ILogger<AppointmentController> logger)
    : ControllerBase
{
    /// <summary>
    /// Retrieves a list of appointments with optional pagination.
    /// </summary>
    /// <param name="skip">Number of appointments to skip for pagination.</param>
    /// <param name="take">Number of appointments to take for pagination.</param>
    /// <returns>A list of appointments.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAllAppointments(int skip = 0, int take = 100)
    {
        logger.LogInformation("Getting appointments with skip: {Skip}, take: {Take}", skip, take);
        try
        {
            var appointments = await appointmentRepository.GetAllAppointmentsAsync(skip, take);
            logger.LogInformation("Retrieved {Count} appointments", appointments.Count());
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting appointments");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    /// <summary>
    /// Retrieves an appointment by its ID.
    /// </summary>
    /// <param name="id">The ID of the appointment to retrieve.</param>
    /// <returns>The appointment with the specified ID.</returns>
    [HttpGet("{id:long}")]
    [ResponseCache(Duration = 60, VaryByQueryKeys = ["id"])]
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

    /// <summary>
    /// Creates a new appointment.
    /// </summary>
    /// <param name="appointment">The appointment to create.</param>
    /// <returns>The created appointment.</returns>
    [HttpPost]
    public async Task<ActionResult<Appointment?>> CreateAppointment(Appointment? appointment)
    {
        logger.LogInformation("Creating new appointment");
        var createdAppointment = await appointmentRepository.CreateAppointmentAsync(appointment);
        logger.LogInformation("Created appointment with id: {Id}", createdAppointment?.AppointmentId);
        return CreatedAtAction(nameof(GetAppointmentById), new { id = createdAppointment?.AppointmentId }, createdAppointment);
    }

    /// <summary>
    /// Updates an existing appointment.
    /// </summary>
    /// <param name="id">The ID of the appointment to update.</param>
    /// <param name="appointment">The updated appointment details.</param>
    /// <returns>No content if the update is successful.</returns>
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

    /// <summary>
    /// Deletes an appointment by its ID.
    /// </summary>
    /// <param name="id">The ID of the appointment to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAppointment(long id)
    {
        logger.LogInformation("Deleting appointment with id: {Id}", id);
        await appointmentRepository.DeleteAppointmentAsync(id);
        logger.LogInformation("Deleted appointment with id: {Id}", id);
        return NoContent();
    }
}
