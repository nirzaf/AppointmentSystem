using AppointmentSystem.Data;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Repositories.Implementation;

/// <summary>
/// Repository class for managing appointments in the database.
/// </summary>
public class AppointmentRepository(ClinicDbContext context, ILogger<AppointmentRepository> logger)
    : IAppointmentRepository
{
    private readonly ClinicDbContext context;
    private readonly ILogger<AppointmentRepository> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppointmentRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to be used for data access.</param>
    /// <param name="logger">The logger to be used for logging information and errors.</param>
    public AppointmentRepository(ClinicDbContext context, ILogger<AppointmentRepository> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves a list of appointments with optional pagination.
    /// </summary>
    /// <param name="skip">Number of appointments to skip for pagination.</param>
    /// <param name="take">Number of appointments to take for pagination.</param>
    /// <returns>A list of appointments.</returns>
    public async Task<IEnumerable<Appointment?>> GetAllAppointmentsAsync(int skip = 0, int take = 100)
    {
        try
        {
            logger.LogInformation("Getting appointments with skip: {Skip}, take: {Take}", skip, take);
            var appointments = await context.Appointments
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            logger.LogInformation("Retrieved {Count} appointments", appointments.Count());
            return appointments;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting appointments");
            throw;
        }
    }

    /// <summary>
    /// Retrieves an appointment by its ID.
    /// </summary>
    /// <param name="id">The ID of the appointment to retrieve.</param>
    /// <returns>The appointment with the specified ID.</returns>
    public async Task<Appointment?> GetAppointmentByIdAsync(long id)
    {
        try
        {
            logger.LogInformation("Getting appointment with id: {AppointmentId}", id);
            var appointment = await context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                logger.LogWarning("Appointment with id: {AppointmentId} not found", id);
            }
            else
            {
                logger.LogInformation("Retrieved appointment with id: {AppointmentId}", id);
            }
            return appointment;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting appointment with id {AppointmentId}", id);
            throw;
        }
    }

    /// <summary>
    /// Creates a new appointment.
    /// </summary>
    /// <param name="appointment">The appointment to create.</param>
    /// <returns>The created appointment.</returns>
    public async Task<Appointment?> CreateAppointmentAsync(Appointment? appointment)
    {
        try
        {
            if (appointment != null)
            {
                logger.LogInformation("Creating new appointment");
                context.Appointments.Add(appointment);
                await context.SaveChangesAsync();
                logger.LogInformation("Created appointment with id: {AppointmentId}", appointment.AppointmentId);
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

    /// <summary>
    /// Updates an existing appointment.
    /// </summary>
    /// <param name="appointment">The updated appointment details.</param>
    /// <returns>The updated appointment.</returns>
    public async Task<Appointment?> UpdateAppointmentAsync(Appointment appointment)
    {
        try
        {
            logger.LogInformation("Updating appointment with id: {AppointmentId}", appointment.AppointmentId);
            context.Entry(appointment).State = EntityState.Modified;
            await context.SaveChangesAsync();
            logger.LogInformation("Updated appointment with id: {AppointmentId}", appointment.AppointmentId);
            return appointment;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating appointment with id {AppointmentId}", appointment.AppointmentId);
            throw;
        }
    }

    /// <summary>
    /// Deletes an appointment by its ID.
    /// </summary>
    /// <param name="id">The ID of the appointment to delete.</param>
    public async Task DeleteAppointmentAsync(long id)
    {
        try
        {
            logger.LogInformation("Deleting appointment with id: {AppointmentId}", id);
            var appointment = await context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                context.Appointments.Remove(appointment);
                await context.SaveChangesAsync();
                logger.LogInformation("Deleted appointment with id: {AppointmentId}", id);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting appointment with id {AppointmentId}", id);
            throw;
        }
    }
}