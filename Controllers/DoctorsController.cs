using Microsoft.AspNetCore.Mvc;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;

namespace AppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController(IDoctorRepository doctorRepository, ILogger logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Doctor>>> GetAllDoctors()
    {
        logger.Information("Getting all doctors");
        var doctors = await doctorRepository.GetAllDoctorsAsync();
        logger.Information("Retrieved {Count} doctors", doctors.Count());
        return Ok(doctors);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Doctor?>> GetDoctorById(long id)
    {
        logger.Information("Getting doctor with ID: {DoctorId}", id);
        var doctor = await doctorRepository.GetDoctorByIdAsync(id);
        if (doctor == null)
        {
            logger.Warning("Doctor with ID: {DoctorId} not found", id);
            return NotFound();
        }
        logger.Information("Retrieved doctor with ID: {DoctorId}", id);
        return Ok(doctor);
    }

    [HttpPost]
    public async Task<ActionResult<Doctor>> CreateDoctor(Doctor doctor)
    {
        logger.Information("Creating new doctor");
        var createdDoctor = await doctorRepository.AddDoctorAsync(doctor);
        logger.Information("Created doctor with ID: {DoctorId}", createdDoctor.DoctorId);
        return CreatedAtAction(nameof(GetDoctorById), new { id = createdDoctor.DoctorId }, createdDoctor);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateDoctor(long id, Doctor doctor)
    {
        logger.Information("Updating doctor with ID: {DoctorId}", id);
        if (id != doctor.DoctorId)
        {
            logger.Warning("Bad request: ID mismatch for doctor update");
            return BadRequest();
        }

        await doctorRepository.UpdateDoctorAsync(doctor);
        logger.Information("Updated doctor with ID: {DoctorId}", id);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteDoctor(long id)
    {
        logger.Information("Deleting doctor with ID: {DoctorId}", id);
        await doctorRepository.DeleteDoctorAsync(id);
        logger.Information("Deleted doctor with ID: {DoctorId}", id);
        return NoContent();
    }
}