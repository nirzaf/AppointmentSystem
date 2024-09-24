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
            // Log the request for getting appointments with pagination
            logger.LogInformation("Getting appointments with skip: {Skip}, take: {Take}", skip, take);
            
            // Fetch appointments from the database with pagination
            var appointments = await context.Appointments
                .Skip(skip) // Skip the specified number of appointments
                .Take(take) // Take the specified number of appointments
                .ToListAsync(); // Convert the result to a list
            
            // Log the number of appointments retrieved
            logger.LogInformation("Retrieved {Count} appointments", appointments.Count());
            
            // Return the list of appointments
            return appointments;
        }
        catch (Exception ex)
        {
            // Log the error if an exception occurs
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
            // Log the request for getting an appointment by ID
            logger.LogInformation("Getting appointment with id: {AppointmentId}", id);
            
            // Fetch the appointment from the database by ID
            var appointment = await context.Appointments.FindAsync(id);
            
            if (appointment == null)
            {
                // Log a warning if the appointment is not found
                logger.LogWarning("Appointment with id: {AppointmentId} not found", id);
            }
            else
            {
                // Log the retrieved appointment details
                logger.LogInformation("Retrieved appointment with id: {AppointmentId}", id);
            }
            
            // Return the appointment details
            return appointment;
        }
        catch (Exception ex)
        {
            // Log the error if an exception occurs
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
                // Log the request for creating a new appointment
                logger.LogInformation("Creating new appointment");
                
                // Add the new appointment to the database
                context.Appointments.Add(appointment);
                await context.SaveChangesAsync(); // Save the changes to the database
                
                // Log the ID of the created appointment
                logger.LogInformation("Created appointment with id: {AppointmentId}", appointment.AppointmentId);
                
                // Return the created appointment details
                return appointment;
            }
            return null;
        }
        catch (Exception ex)
        {
            // Log the error if an exception occurs
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
            // Log the request for updating an appointment
            logger.LogInformation("Updating appointment with id: {AppointmentId}", appointment.AppointmentId);
            
            // Mark the appointment entity as modified
            context.Entry(appointment).State = EntityState.Modified;
            await context.SaveChangesAsync(); // Save the changes to the database
            
            // Log the ID of the updated appointment
            logger.LogInformation("Updated appointment with id: {AppointmentId}", appointment.AppointmentId);
            
            // Return the updated appointment details
            return appointment;
        }
        catch (Exception ex)
        {
            // Log the error if an exception occurs
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
            // Log the request for deleting an appointment
            logger.LogInformation("Deleting appointment with id: {AppointmentId}", id);
            
            // Fetch the appointment from the database by ID
            var appointment = await context.Appointments.FindAsync(id);
            
            if (appointment != null)
            {
                // Remove the appointment from the database
                context.Appointments.Remove(appointment);
                await context.SaveChangesAsync(); // Save the changes to the database
                
                // Log the ID of the deleted appointment
                logger.LogInformation("Deleted appointment with id: {AppointmentId}", id);
            }
        }
        catch (Exception ex)
        {
            // Log the error if an exception occurs
            logger.LogError(ex, "An error occurred while deleting appointment with id {AppointmentId}", id);
            throw;
        }
    }
}
