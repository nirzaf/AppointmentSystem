using AppointmentSystem.Data;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Repositories;

public class ClinicRepository : IClinicRepository
{
    private readonly ClinicDbContext _context;

    public ClinicRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Clinic>> GetAllClinicsAsync()
    {
        return await _context.Clinics.ToListAsync();
    }

    public async Task<Clinic> GetClinicByIdAsync(int id)
    {
        return await _context.Clinics.FindAsync(id);
    }

    public async Task<Clinic> AddClinicAsync(Clinic clinic)
    {
        _context.Clinics.Add(clinic);
        await _context.SaveChangesAsync();
        return clinic;
    }

    public async Task UpdateClinicAsync(Clinic clinic)
    {
        _context.Entry(clinic).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteClinicAsync(int id)
    {
        var clinic = await _context.Clinics.FindAsync(id);
        if (clinic != null)
        {
            _context.Clinics.Remove(clinic);
            await _context.SaveChangesAsync();
        }
    }
}