using AppointmentSystem.Data;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Repositories;

public class PatientRepository(ClinicDbContext context, ILogger<PatientRepository> logger) : IPatientRepository
{
    public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
    {
        try
        {
            return await context.Patients.ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting all patients");
            throw;
        }
    }

    public async Task<Patient?> GetPatientByIdAsync(long id)
    {
        try
        {
            return await context.Patients.FindAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting patient with id {PatientId}", id);
            throw;
        }
    }

    public async Task<Patient> AddPatientAsync(Patient patient)
    {
        try
        {
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
            return patient;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while adding a new patient");
            throw;
        }
    }

    public async Task UpdatePatientAsync(Patient patient)
    {
        try
        {
            context.Entry(patient).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating patient with id {PatientId}", patient.PatientId);
            throw;
        }
    }

    public async Task DeletePatientAsync(long id)
    {
        try
        {
            var patient = await context.Patients.FindAsync(id);
            if (patient != null)
            {
                context.Patients.Remove(patient);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting patient with id {PatientId}", id);
            throw;
        }
    }
}