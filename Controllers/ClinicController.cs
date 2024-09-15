using Microsoft.AspNetCore.Mvc;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;

namespace AppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClinicController : ControllerBase
{
    private readonly IClinicRepository _clinicRepository;

    public ClinicController(IClinicRepository clinicRepository)
    {
        _clinicRepository = clinicRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Clinic>>> GetAllClinics()
    {
        var clinics = await _clinicRepository.GetAllClinicsAsync();
        return Ok(clinics);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Clinic>> GetClinicById(int id)
    {
        var clinic = await _clinicRepository.GetClinicByIdAsync(id);
        if (clinic == null)
        {
            return NotFound();
        }
        return Ok(clinic);
    }

    [HttpPost]
    public async Task<ActionResult<Clinic>> CreateClinic(Clinic clinic)
    {
        var createdClinic = await _clinicRepository.AddClinicAsync(clinic);
        return CreatedAtAction(nameof(GetClinicById), new { id = createdClinic.ClinicId }, createdClinic);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClinic(int id, Clinic clinic)
    {
        if (id != clinic.ClinicId)
        {
            return BadRequest();
        }

        await _clinicRepository.UpdateClinicAsync(clinic);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClinic(int id)
    {
        await _clinicRepository.DeleteClinicAsync(id);
        return NoContent();
    }
}