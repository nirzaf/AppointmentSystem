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
        // Log the request for getting patients with pagination
        logger.LogInformation("Getting patients with pagination: Skip {Skip}, Take {Take}", skip, take);
        
        // Fetch all patients from the repository
        var patients = await patientRepository.GetAllPatientsAsync();
        
        // Apply pagination to the result
        var paginatedPatients = patients.Skip(skip).Take(take);
        
        // Convert the result to an array to avoid multiple enumerations
        var enumerable = paginatedPatients as Patient[] ?? paginatedPatients.ToArray();
        
        // Log the number of patients retrieved
        logger.LogInformation("Retrieved {Count} patients", enumerable.Length);
        
        // Return the list of patients
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
        // Log the request for getting a patient by ID
        logger.LogInformation("Getting patient with ID: {PatientId}", id);
        
        // Fetch the patient from the repository by ID
        var patient = await patientRepository.GetPatientByIdAsync(id);
        
        if (patient == null)
        {
            // Log a warning if the patient is not found
            logger.LogWarning("Patient with ID: {PatientId} not found", id);
            return NotFound();
        }
        
        // Log the retrieved patient details
        logger.LogInformation("Retrieved patient with ID: {PatientId}", id);
        
        // Return the patient details
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
        // Log the request for creating a new patient
        logger.LogInformation("Creating new patient");
        
        // Add the new patient to the repository
        var createdPatient = await patientRepository.AddPatientAsync(patient);
        
        // Log the ID of the created patient
        logger.LogInformation("Created patient with ID: {PatientId}", createdPatient.PatientId);
        
        // Return the created patient details
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
        // Log the request for updating a patient
        logger.LogInformation("Updating patient with ID: {PatientId}", id);
        
        if (id != patient.PatientId)
        {
            // Log a warning if the provided ID does not match the patient ID
            logger.LogWarning("Bad request: ID mismatch for patient update");
            return BadRequest();
        }

        // Update the patient in the repository
        await patientRepository.UpdatePatientAsync(patient);
        
        // Log the ID of the updated patient
        logger.LogInformation("Updated patient with ID: {PatientId}", id);
        
        // Return no content to indicate a successful update
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
        // Log the request for deleting a patient
        logger.LogInformation("Deleting patient with ID: {PatientId}", id);
        
        // Delete the patient from the repository by ID
        await patientRepository.DeletePatientAsync(id);
        
        // Log the ID of the deleted patient
        logger.LogInformation("Deleted patient with ID: {PatientId}", id);
        
        // Return no content to indicate a successful deletion
        return NoContent();
    }
}
