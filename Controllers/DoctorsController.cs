using Microsoft.AspNetCore.Mvc;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;

namespace AppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController(IDoctorRepository doctorRepository, ILogger<DoctorsController> logger)
    : ControllerBase
{
    /// <summary>
    /// Retrieves a list of doctors with optional pagination.
    /// </summary>
    /// <param name="skip">Number of doctors to skip for pagination.</param>
    /// <param name="take">Number of doctors to take for pagination.</param>
    /// <returns>A list of doctors.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Doctor>>> GetAllDoctors([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        // Log the request for getting doctors with pagination
        logger.LogInformation("Getting doctors with skip: {Skip} and take: {Take}", skip, take);
        
        // Fetch all doctors from the repository
        var doctors = await doctorRepository.GetAllDoctorsAsync();
        
        // Convert the result to an array to avoid multiple enumerations
        var enumerable = doctors as Doctor[] ?? doctors.ToArray();
        
        // Apply pagination to the result
        var result = enumerable.Skip(skip).Take(take);
        
        // Log the number of doctors retrieved
        logger.LogInformation("Retrieved {Count} doctors", result.Count());
        
        // Return the list of doctors
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a doctor by its ID.
    /// </summary>
    /// <param name="id">The ID of the doctor to retrieve.</param>
    /// <returns>The doctor with the specified ID.</returns>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<Doctor?>> GetDoctorById(long id)
    {
        // Log the request for getting a doctor by ID
        logger.LogInformation("Getting doctor with ID: {DoctorId}", id);
        
        // Fetch the doctor from the repository by ID
        var doctor = await doctorRepository.GetDoctorByIdAsync(id);
        
        if (doctor == null)
        {
            // Log a warning if the doctor is not found
            logger.LogWarning("Doctor with ID: {DoctorId} not found", id);
            return NotFound();
        }
        
        // Log the retrieved doctor details
        logger.LogInformation("Retrieved doctor with ID: {DoctorId}", id);
        
        // Return the doctor details
        return Ok(doctor);
    }

    /// <summary>
    /// Creates a new doctor.
    /// </summary>
    /// <param name="doctor">The doctor to create.</param>
    /// <returns>The created doctor.</returns>
    [HttpPost]
    public async Task<ActionResult<Doctor>> CreateDoctor(Doctor doctor)
    {
        // Log the request for creating a new doctor
        logger.LogInformation("Creating new doctor");
        
        // Add the new doctor to the repository
        var createdDoctor = await doctorRepository.AddDoctorAsync(doctor);
        
        // Log the ID of the created doctor
        logger.LogInformation("Created doctor with ID: {DoctorId}", createdDoctor.DoctorId);
        
        // Return the created doctor details
        return CreatedAtAction(nameof(GetDoctorById), new { id = createdDoctor.DoctorId }, createdDoctor);
    }

    /// <summary>
    /// Updates an existing doctor.
    /// </summary>
    /// <param name="id">The ID of the doctor to update.</param>
    /// <param name="doctor">The updated doctor details.</param>
    /// <returns>No content if the update is successful.</returns>
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateDoctor(long id, Doctor doctor)
    {
        // Log the request for updating a doctor
        logger.LogInformation("Updating doctor with ID: {DoctorId}", id);
        
        if (id != doctor.DoctorId)
        {
            // Log a warning if the provided ID does not match the doctor ID
            logger.LogWarning("Bad request: ID mismatch for doctor update");
            return BadRequest();
        }

        // Update the doctor in the repository
        await doctorRepository.UpdateDoctorAsync(doctor);
        
        // Log the ID of the updated doctor
        logger.LogInformation("Updated doctor with ID: {DoctorId}", id);
        
        // Return no content to indicate a successful update
        return NoContent();
    }

    /// <summary>
    /// Deletes a doctor by its ID.
    /// </summary>
    /// <param name="id">The ID of the doctor to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteDoctor(long id)
    {
        // Log the request for deleting a doctor
        logger.LogInformation("Deleting doctor with ID: {DoctorId}", id);
        
        // Delete the doctor from the repository by ID
        await doctorRepository.DeleteDoctorAsync(id);
        
        // Log the ID of the deleted doctor
        logger.LogInformation("Deleted doctor with ID: {DoctorId}", id);
        
        // Return no content to indicate a successful deletion
        return NoContent();
    }
}
