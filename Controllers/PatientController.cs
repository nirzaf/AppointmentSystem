using Microsoft.AspNetCore.Mvc;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;

namespace AppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController(IPatientRepository patientRepository, ILogger<PatientController> logger)
    : ControllerBase
{
    /// <summary>
    /// Retrieves a list of patients with optional pagination.
    /// </summary>
    /// <param name="skip">Number of patients to skip for pagination.</param>
    /// <param name="take">Number of patients to take for pagination.</param>
    /// <returns>A list of patients.</returns>
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

    /// <summary>
    /// Retrieves a patient by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient to retrieve.</param>
    /// <returns>The patient with the specified ID.</returns>
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

    /// <summary>
    /// Creates a new patient.
    /// </summary>
    /// <param name="patient">The patient to create.</param>
    /// <returns>The created patient.</returns>
    [HttpPost]
    public async Task<ActionResult<Patient>> CreatePatient(Patient patient)
    {
        logger.LogInformation("Creating new patient");
        var createdPatient = await patientRepository.AddPatientAsync(patient);
        logger.LogInformation("Created patient with ID: {PatientId}", createdPatient.PatientId);
        return CreatedAtAction(nameof(GetPatientById), new { id = createdPatient.PatientId }, createdPatient);
    }

    /// <summary>
    /// Updates an existing patient.
    /// </summary>
    /// <param name="id">The ID of the patient to update.</param>
    /// <param name="patient">The updated patient details.</param>
    /// <returns>No content if the update is successful.</returns>
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

    /// <summary>
    /// Deletes a patient by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeletePatient(long id)
    {
        logger.LogInformation("Deleting patient with ID: {PatientId}", id);
        await patientRepository.DeletePatientAsync(id);
        logger.LogInformation("Deleted patient with ID: {PatientId}", id);
        return NoContent();
    }
}