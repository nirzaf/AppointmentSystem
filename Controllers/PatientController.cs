using Microsoft.AspNetCore.Mvc;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;

namespace AppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController(IPatientRepository patientRepository, ILogger logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetAllPatients()
    {
        logger.Information("Getting all patients");
        var patients = await patientRepository.GetAllPatientsAsync();
        logger.Information("Retrieved {Count} patients", patients.Count());
        return Ok(patients);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Patient?>> GetPatientById(long id)
    {
        logger.Information("Getting patient with ID: {PatientId}", id);
        var patient = await patientRepository.GetPatientByIdAsync(id);
        if (patient == null)
        {
            logger.Warning("Patient with ID: {PatientId} not found", id);
            return NotFound();
        }
        logger.Information("Retrieved patient with ID: {PatientId}", id);
        return Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> CreatePatient(Patient patient)
    {
        logger.Information("Creating new patient");
        var createdPatient = await patientRepository.AddPatientAsync(patient);
        logger.Information("Created patient with ID: {PatientId}", createdPatient.PatientId);
        return CreatedAtAction(nameof(GetPatientById), new { id = createdPatient.PatientId }, createdPatient);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdatePatient(long id, Patient patient)
    {
        logger.Information("Updating patient with ID: {PatientId}", id);
        if (id != patient.PatientId)
        {
            logger.Warning("Bad request: ID mismatch for patient update");
            return BadRequest();
        }

        await patientRepository.UpdatePatientAsync(patient);
        logger.Information("Updated patient with ID: {PatientId}", id);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeletePatient(long id)
    {
        logger.Information("Deleting patient with ID: {PatientId}", id);
        await patientRepository.DeletePatientAsync(id);
        logger.Information("Deleted patient with ID: {PatientId}", id);
        return NoContent();
    }
}