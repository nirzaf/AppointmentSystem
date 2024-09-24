using Microsoft.AspNetCore.Mvc;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;

namespace AppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClinicController(IClinicRepository clinicRepository, ILogger<ClinicController> logger)
    : ControllerBase
{
    /// <summary>
    /// Retrieves a list of clinics with optional pagination.
    /// </summary>
    /// <param name="skip">Number of clinics to skip for pagination.</param>
    /// <param name="take">Number of clinics to take for pagination.</param>
    /// <returns>A list of clinics.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Clinic>>> GetAllClinics([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        logger.LogInformation("Getting clinics with pagination: Skip {Skip}, Take {Take}", skip, take);
        var clinics = await clinicRepository.GetAllClinicsAsync(skip, take);
        logger.LogInformation("Retrieved {Count} clinics", clinics.Count());
        return Ok(clinics);
    }

    /// <summary>
    /// Retrieves a clinic by its ID.
    /// </summary>
    /// <param name="id">The ID of the clinic to retrieve.</param>
    /// <returns>The clinic with the specified ID.</returns>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<Clinic?>> GetClinicById(long id)
    {
        logger.LogInformation("Getting clinic with id: {ClinicId}", id);
        var clinic = await clinicRepository.GetClinicByIdAsync(id);
        if (clinic == null)
        {
            logger.LogWarning("Clinic with id: {ClinicId} not found", id);
            return NotFound();
        }
        logger.LogInformation("Retrieved clinic: {@Clinic}", clinic);
        return Ok(clinic);
    }

    /// <summary>
    /// Creates a new clinic.
    /// </summary>
    /// <param name="clinic">The clinic to create.</param>
    /// <returns>The created clinic.</returns>
    [HttpPost]
    public async Task<ActionResult<Clinic?>> CreateClinic(Clinic? clinic)
    {
        logger.LogInformation("Creating new clinic: {@Clinic}", clinic);
        var createdClinic = await clinicRepository.AddClinicAsync(clinic);
        logger.LogInformation("Created new clinic with id: {ClinicId}", createdClinic?.ClinicId);
        return CreatedAtAction(nameof(GetClinicById), new { id = createdClinic?.ClinicId }, createdClinic);
    }

    /// <summary>
    /// Updates an existing clinic.
    /// </summary>
    /// <param name="id">The ID of the clinic to update.</param>
    /// <param name="clinic">The updated clinic details.</param>
    /// <returns>No content if the update is successful.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClinic(long id, Clinic clinic)
    {
        if (id != clinic.ClinicId)
        {
            logger.LogWarning("Mismatched clinic id. Provided: {ProvidedId}, Actual: {ActualId}", id, clinic.ClinicId);
            return BadRequest();
        }

        logger.LogInformation("Updating clinic: {@Clinic}", clinic);
        await clinicRepository.UpdateClinicAsync(clinic);
        logger.LogInformation("Updated clinic with id: {ClinicId}", id);
        return NoContent();
    }

    /// <summary>
    /// Deletes a clinic by its ID.
    /// </summary>
    /// <param name="id">The ID of the clinic to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteClinic(long id)
    {
        logger.LogInformation("Deleting clinic with id: {ClinicId}", id);
        await clinicRepository.DeleteClinicAsync(id);
        logger.LogInformation("Deleted clinic with id: {ClinicId}", id);
        return NoContent();
    }
}