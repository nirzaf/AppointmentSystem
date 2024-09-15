using Microsoft.AspNetCore.Mvc;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;

namespace AppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController(IPatientRepository patientRepository, ILogger<PatientController> logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetAllPatients([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        logger.LogInformation("Getting patients with pagination: Skip {Skip}, Take {Take}", skip, take);
        var patients = await patientRepository.GetAllPatientsAsync();
        var paginatedPatients = patients.Skip(skip).Take(take);
        var enumerable = paginatedPatients as Patient[] ?? paginatedPatients.ToArray();
        logger.LogInformation("Retrieved {Count} patients", enumerable.Length);
        return Ok(enumerable);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Patient?>> GetPatientById(long id)
    {
        logger.LogInformation("Getting patient with ID: {PatientId}", id);
        var patient = await patientRepository.GetPatientByIdAsync(id);
        if (patient == null)
        {
            logger.LogWarning("Patient with ID: {PatientId} not found", id);
            return NotFound();
        }
        logger.LogInformation("Retrieved patient with ID: {PatientId}", id);
        return Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> CreatePatient(Patient patient)
    {
        logger.LogInformation("Creating new patient");
        var createdPatient = await patientRepository.AddPatientAsync(patient);
        logger.LogInformation("Created patient with ID: {PatientId}", createdPatient.PatientId);
        return CreatedAtAction(nameof(GetPatientById), new { id = createdPatient.PatientId }, createdPatient);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdatePatient(long id, Patient patient)
    {
        logger.LogInformation("Updating patient with ID: {PatientId}", id);
        if (id != patient.PatientId)
        {
            logger.LogWarning("Bad request: ID mismatch for patient update");
            return BadRequest();
        }

        await patientRepository.UpdatePatientAsync(patient);
        logger.LogInformation("Updated patient with ID: {PatientId}", id);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeletePatient(long id)
    {
        logger.LogInformation("Deleting patient with ID: {PatientId}", id);
        await patientRepository.DeletePatientAsync(id);
        logger.LogInformation("Deleted patient with ID: {PatientId}", id);
        return NoContent();
    }
}