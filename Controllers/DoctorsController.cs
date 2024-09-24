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
        logger.LogInformation("Getting doctors with skip: {Skip} and take: {Take}", skip, take);
        var doctors = await doctorRepository.GetAllDoctorsAsync();
        var enumerable = doctors as Doctor[] ?? doctors.ToArray();
        var result = enumerable.Skip(skip).Take(take);
        logger.LogInformation("Retrieved {Count} doctors", result.Count());
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
        logger.LogInformation("Getting doctor with ID: {DoctorId}", id);
        var doctor = await doctorRepository.GetDoctorByIdAsync(id);
        if (doctor == null)
        {
            logger.LogWarning("Doctor with ID: {DoctorId} not found", id);
            return NotFound();
        }
        logger.LogInformation("Retrieved doctor with ID: {DoctorId}", id);
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
        logger.LogInformation("Creating new doctor");
        var createdDoctor = await doctorRepository.AddDoctorAsync(doctor);
        logger.LogInformation("Created doctor with ID: {DoctorId}", createdDoctor.DoctorId);
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
        logger.LogInformation("Updating doctor with ID: {DoctorId}", id);
        if (id != doctor.DoctorId)
        {
            logger.LogWarning("Bad request: ID mismatch for doctor update");
            return BadRequest();
        }

        await doctorRepository.UpdateDoctorAsync(doctor);
        logger.LogInformation("Updated doctor with ID: {DoctorId}", id);
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
        logger.LogInformation("Deleting doctor with ID: {DoctorId}", id);
        await doctorRepository.DeleteDoctorAsync(id);
        logger.LogInformation("Deleted doctor with ID: {DoctorId}", id);
        return NoContent();
    }
}
