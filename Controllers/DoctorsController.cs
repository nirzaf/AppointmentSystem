using Microsoft.AspNetCore.Mvc;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;

namespace AppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorRepository _doctorRepository;

    public DoctorsController(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Doctor>>> GetAllDoctors()
    {
        var doctors = await _doctorRepository.GetAllDoctorsAsync();
        return Ok(doctors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Doctor>> GetDoctorById(int id)
    {
        var doctor = await _doctorRepository.GetDoctorByIdAsync(id);
        if (doctor == null)
        {
            return NotFound();
        }
        return Ok(doctor);
    }

    [HttpPost]
    public async Task<ActionResult<Doctor>> CreateDoctor(Doctor doctor)
    {
        var createdDoctor = await _doctorRepository.AddDoctorAsync(doctor);
        return CreatedAtAction(nameof(GetDoctorById), new { id = createdDoctor.DoctorId }, createdDoctor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDoctor(int id, Doctor doctor)
    {
        if (id != doctor.DoctorId)
        {
            return BadRequest();
        }

        await _doctorRepository.UpdateDoctorAsync(doctor);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDoctor(int id)
    {
        await _doctorRepository.DeleteDoctorAsync(id);
        return NoContent();
    }
}