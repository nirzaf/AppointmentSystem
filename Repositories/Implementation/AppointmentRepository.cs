using AppointmentSystem.Data;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Repositories.Implementation;

public class AppointmentRepository(ClinicDbContext context, ILogger<AppointmentRepository> logger)
    : IAppointmentRepository
{
    public async Task<IEnumerable<Appointment?>> GetAllAppointmentsAsync(int skip = 0, int take = 100)
    {
        try
        {
            return await context.Appointments
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting appointments");
            throw;
        }
    }

    public async Task<Appointment?> GetAppointmentByIdAsync(long id)
    {
        try
        {
            return await context.Appointments.FindAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting appointment with id {AppointmentId}", id);
            throw;
        }
    }

    public async Task<Appointment?> CreateAppointmentAsync(Appointment? appointment)
    {
        try
        {
            if (appointment != null)
            {
                context.Appointments.Add(appointment);
                await context.SaveChangesAsync();
                return appointment;
            }
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating an appointment");
            throw;
        }
    }

    public async Task<Appointment?> UpdateAppointmentAsync(Appointment appointment)
    {
        try
        {
            context.Entry(appointment).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return appointment;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating appointment with id {AppointmentId}", appointment.AppointmentId);
            throw;
        }
    }

    public async Task DeleteAppointmentAsync(long id)
    {
        try
        {
            var appointment = await context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                context.Appointments.Remove(appointment);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting appointment with id {AppointmentId}", id);
            throw;
        }
    }
}