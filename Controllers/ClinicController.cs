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
        // Log the request for getting clinics with pagination
        logger.LogInformation("Getting clinics with pagination: Skip {Skip}, Take {Take}", skip, take);
        
        // Fetch clinics from the repository with pagination
        var clinics = await clinicRepository.GetAllClinicsAsync(skip, take);
        
        // Log the number of clinics retrieved
        logger.LogInformation("Retrieved {Count} clinics", clinics.Count());
        
        // Return the list of clinics
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
        // Log the request for getting a clinic by ID
        logger.LogInformation("Getting clinic with id: {ClinicId}", id);
        
        // Fetch the clinic from the repository by ID
        var clinic = await clinicRepository.GetClinicByIdAsync(id);
        
        if (clinic == null)
        {
            // Log a warning if the clinic is not found
            logger.LogWarning("Clinic with id: {ClinicId} not found", id);
            return NotFound();
        }
        
        // Log the retrieved clinic details
        logger.LogInformation("Retrieved clinic: {@Clinic}", clinic);
        
        // Return the clinic details
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
        // Log the request for creating a new clinic
        logger.LogInformation("Creating new clinic: {@Clinic}", clinic);
        
        // Add the new clinic to the repository
        var createdClinic = await clinicRepository.AddClinicAsync(clinic);
        
        // Log the ID of the created clinic
        logger.LogInformation("Created new clinic with id: {ClinicId}", createdClinic?.ClinicId);
        
        // Return the created clinic details
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
            // Log a warning if the provided ID does not match the clinic ID
            logger.LogWarning("Mismatched clinic id. Provided: {ProvidedId}, Actual: {ActualId}", id, clinic.ClinicId);
            return BadRequest();
        }

        // Log the request for updating a clinic
        logger.LogInformation("Updating clinic: {@Clinic}", clinic);
        
        // Update the clinic in the repository
        await clinicRepository.UpdateClinicAsync(clinic);
        
        // Log the ID of the updated clinic
        logger.LogInformation("Updated clinic with id: {ClinicId}", id);
        
        // Return no content to indicate a successful update
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
        // Log the request for deleting a clinic
        logger.LogInformation("Deleting clinic with id: {ClinicId}", id);
        
        // Delete the clinic from the repository by ID
        await clinicRepository.DeleteClinicAsync(id);
        
        // Log the ID of the deleted clinic
        logger.LogInformation("Deleted clinic with id: {ClinicId}", id);
        
        // Return no content to indicate a successful deletion
        return NoContent();
    }
}
