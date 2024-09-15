using AppointmentSystem.Data;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Repositories;

public class AppointmentRepository(ClinicDbContext context, ILogger<AppointmentRepository> logger)
    : IAppointmentRepository
{
    public async Task<List<Appointment?>> GetAllAppointmentsAsync()
    {
        try
        {
            return await context.Appointments.ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting all appointments");
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
            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();
            return appointment;
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