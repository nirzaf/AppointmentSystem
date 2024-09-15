using Microsoft.AspNetCore.Mvc;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;

namespace AppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ClinicController(IClinicRepository clinicRepository, ILogger logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Clinic>>> GetAllClinics()
    {
        logger.Information("Getting all clinics");
        var clinics = await clinicRepository.GetAllClinicsAsync();
        logger.Information("Retrieved {Count} clinics", clinics.Count());
        return Ok(clinics);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Clinic?>> GetClinicById(long id)
    {
        logger.Information("Getting clinic with id: {ClinicId}", id);
        var clinic = await clinicRepository.GetClinicByIdAsync(id);
        if (clinic == null)
        {
            logger.Warning("Clinic with id: {ClinicId} not found", id);
            return NotFound();
        }
        logger.Information("Retrieved clinic: {@Clinic}", clinic);
        return Ok(clinic);
    }

    [HttpPost]
    public async Task<ActionResult<Clinic?>> CreateClinic(Clinic? clinic)
    {
        logger.Information("Creating new clinic: {@Clinic}", clinic);
        var createdClinic = await clinicRepository.AddClinicAsync(clinic);
        logger.Information("Created new clinic with id: {ClinicId}", createdClinic?.ClinicId);
        return CreatedAtAction(nameof(GetClinicById), new { id = createdClinic?.ClinicId }, createdClinic);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClinic(long id, Clinic clinic)
    {
        if (id != clinic.ClinicId)
        {
            logger.Warning("Mismatched clinic id. Provided: {ProvidedId}, Actual: {ActualId}", id, clinic.ClinicId);
            return BadRequest();
        }

        logger.Information("Updating clinic: {@Clinic}", clinic);
        await clinicRepository.UpdateClinicAsync(clinic);
        logger.Information("Updated clinic with id: {ClinicId}", id);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteClinic(long id)
    {
        logger.Information("Deleting clinic with id: {ClinicId}", id);
        await clinicRepository.DeleteClinicAsync(id);
        logger.Information("Deleted clinic with id: {ClinicId}", id);
        return NoContent();
    }
}