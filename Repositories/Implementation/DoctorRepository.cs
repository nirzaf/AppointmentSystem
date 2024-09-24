using AppointmentSystem.Data;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Repositories.Implementation;

/// <summary>
/// Repository class for managing doctors in the database.
/// </summary>
public class DoctorRepository(ClinicDbContext context, ILogger<DoctorRepository> logger) : IDoctorRepository
{
    /// <summary>
    /// Retrieves a list of doctors with optional pagination.
    /// </summary>
    /// <param name="skip">Number of doctors to skip for pagination.</param>
    /// <param name="take">Number of doctors to take for pagination.</param>
    /// <returns>A list of doctors.</returns>
    public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync(int skip = 0, int take = 100)
    {
        try
        {
            // Log the request for getting doctors with pagination
            logger.LogInformation("Getting doctors with skip: {Skip} and take: {Take}", skip, take);
            
            // Fetch doctors from the database with pagination
            var doctors = await context.Doctors
                .Skip(skip) // Skip the specified number of doctors
                .Take(take) // Take the specified number of doctors
                .ToListAsync(); // Convert the result to a list
            
            // Log the number of doctors retrieved
            logger.LogInformation("Retrieved {Count} doctors", doctors.Count());
            
            // Return the list of doctors
            return doctors;
        }
        catch (Exception ex)
        {
            // Log the error if an exception occurs
            logger.LogError(ex, "An error occurred while getting all doctors");
            throw;
        }
    }

    /// <summary>
    /// Retrieves a doctor by its ID.
    /// </summary>
    /// <param name="id">The ID of the doctor to retrieve.</param>
    /// <returns>The doctor with the specified ID.</returns>
    public async Task<Doctor?> GetDoctorByIdAsync(long id)
    {
        try
        {
            // Log the request for getting a doctor by ID
            logger.LogInformation("Getting doctor with id: {DoctorId}", id);
            
            // Fetch the doctor from the database by ID
            var doctor = await context.Doctors.FindAsync(id);
            
            if (doctor == null)
            {
                // Log a warning if the doctor is not found
                logger.LogWarning("Doctor with id: {DoctorId} not found", id);
            }
            else
            {
                // Log the retrieved doctor details
                logger.LogInformation("Retrieved doctor with id: {DoctorId}", id);
            }
            
            // Return the doctor details
            return doctor;
        }
        catch (Exception ex)
        {
            // Log the error if an exception occurs
            logger.LogError(ex, "An error occurred while getting doctor with id {DoctorId}", id);
            throw;
        }
    }

    /// <summary>
    /// Adds a new doctor.
    /// </summary>
    /// <param name="doctor">The doctor to add.</param>
    /// <returns>The added doctor.</returns>
    public async Task<Doctor> AddDoctorAsync(Doctor doctor)
    {
        try
        {
            // Log the request for adding a new doctor
            logger.LogInformation("Adding new doctor");
            
            // Add the new doctor to the database
            context.Doctors.Add(doctor);
            await context.SaveChangesAsync(); // Save the changes to the database
            
            // Log the ID of the added doctor
            logger.LogInformation("Added doctor with id: {DoctorId}", doctor.DoctorId);
            
            // Return the added doctor details
            return doctor;
        }
        catch (Exception ex)
        {
            // Log the error if an exception occurs
            logger.LogError(ex, "An error occurred while adding a new doctor");
            throw;
        }
    }

    /// <summary>
    /// Updates an existing doctor.
    /// </summary>
    /// <param name="doctor">The updated doctor details.</param>
    public async Task UpdateDoctorAsync(Doctor doctor)
    {
        try
        {
            // Log the request for updating a doctor
            logger.LogInformation("Updating doctor with id: {DoctorId}", doctor.DoctorId);
            
            // Mark the doctor entity as modified
            context.Entry(doctor).State = EntityState.Modified;
            await context.SaveChangesAsync(); // Save the changes to the database
            
            // Log the ID of the updated doctor
            logger.LogInformation("Updated doctor with id: {DoctorId}", doctor.DoctorId);
        }
        catch (Exception ex)
        {
            // Log the error if an exception occurs
            logger.LogError(ex, "An error occurred while updating doctor with id {DoctorId}", doctor.DoctorId);
            throw;
        }
    }

    /// <summary>
    /// Deletes a doctor by its ID.
    /// </summary>
    /// <param name="id">The ID of the doctor to delete.</param>
    public async Task DeleteDoctorAsync(long id)
    {
        try
        {
            // Log the request for deleting a doctor
            logger.LogInformation("Deleting doctor with id: {DoctorId}", id);
            
            // Fetch the doctor from the database by ID
            var doctor = await context.Doctors.FindAsync(id);
            
            if (doctor != null)
            {
                // Remove the doctor from the database
                context.Doctors.Remove(doctor);
                await context.SaveChangesAsync(); // Save the changes to the database
                
                // Log the ID of the deleted doctor
                logger.LogInformation("Deleted doctor with id: {DoctorId}", id);
            }
        }
        catch (Exception ex)
        {
            // Log the error if an exception occurs
            logger.LogError(ex, "An error occurred while deleting doctor with id {DoctorId}", id);
            throw;
        }
    }
}
