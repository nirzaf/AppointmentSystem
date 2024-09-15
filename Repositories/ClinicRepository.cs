using AppointmentSystem.Data;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Repositories;

public class ClinicRepository(ClinicDbContext context, ILogger<ClinicRepository> logger) : IClinicRepository
{
    public async Task<IEnumerable<Clinic?>> GetAllClinicsAsync()
    {
        try
        {
            return await context.Clinics.ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting all clinics");
            throw;
        }
    }

    public async Task<Clinic?> GetClinicByIdAsync(long id)
    {
        try
        {
            return await context.Clinics.FindAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting clinic with id {ClinicId}", id);
            throw;
        }
    }

    public async Task<Clinic?> AddClinicAsync(Clinic? clinic)
    {
        try
        {
            context.Clinics.Add(clinic);
            await context.SaveChangesAsync();
            return clinic;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while adding a new clinic");
            throw;
        }
    }

    public async Task UpdateClinicAsync(Clinic clinic)
    {
        try
        {
            context.Entry(clinic).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating clinic with id {ClinicId}", clinic.ClinicId);
            throw;
        }
    }

    public async Task DeleteClinicAsync(long id)
    {
        try
        {
            var clinic = await context.Clinics.FindAsync(id);
            if (clinic != null)
            {
                context.Clinics.Remove(clinic);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting clinic with id {ClinicId}", id);
            throw;
        }
    }
}