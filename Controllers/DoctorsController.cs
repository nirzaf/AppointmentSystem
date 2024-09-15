using Microsoft.AspNetCore.Mvc;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;

namespace AppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController(IDoctorRepository doctorRepository, ILogger<DoctorsController> logger)
    : ControllerBase
{
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

    [HttpPost]
    public async Task<ActionResult<Doctor>> CreateDoctor(Doctor doctor)
    {
        logger.LogInformation("Creating new doctor");
        var createdDoctor = await doctorRepository.AddDoctorAsync(doctor);
        logger.LogInformation("Created doctor with ID: {DoctorId}", createdDoctor.DoctorId);
        return CreatedAtAction(nameof(GetDoctorById), new { id = createdDoctor.DoctorId }, createdDoctor);
    }

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

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteDoctor(long id)
    {
        logger.LogInformation("Deleting doctor with ID: {DoctorId}", id);
        await doctorRepository.DeleteDoctorAsync(id);
        logger.LogInformation("Deleted doctor with ID: {DoctorId}", id);
        return NoContent();
    }
}