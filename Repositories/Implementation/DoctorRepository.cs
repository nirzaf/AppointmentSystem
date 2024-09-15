using AppointmentSystem.Data;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Repositories.Implementation;

public class DoctorRepository(ClinicDbContext context, ILogger<DoctorRepository> logger) : IDoctorRepository
{
    public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync(int skip = 0, int take = 100)
    {
        try
        {
            return await context.Doctors
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting all doctors");
            throw;
        }
    }
    public async Task<Doctor?> GetDoctorByIdAsync(long id)
    {
        try
        {
            return await context.Doctors.FindAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting doctor with id {DoctorId}", id);
            throw;
        }
    }

    public async Task<Doctor> AddDoctorAsync(Doctor doctor)
    {
        try
        {
            context.Doctors.Add(doctor);
            await context.SaveChangesAsync();
            return doctor;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while adding a new doctor");
            throw;
        }
    }

    public async Task UpdateDoctorAsync(Doctor doctor)
    {
        try
        {
            context.Entry(doctor).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating doctor with id {DoctorId}", doctor.DoctorId);
            throw;
        }
    }

    public async Task DeleteDoctorAsync(long id)
    {
        try
        {
            var doctor = await context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                context.Doctors.Remove(doctor);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting doctor with id {DoctorId}", id);
            throw;
        }
    }
}