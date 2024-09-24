using AppointmentSystem.Data;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Repositories.Implementation;

/// <summary>
/// Repository class for managing clinics in the database.
/// </summary>
public class ClinicRepository(ClinicDbContext context, ILogger<ClinicRepository> logger) : IClinicRepository
{
    private readonly ClinicDbContext context;
    private readonly ILogger<ClinicRepository> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClinicRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to be used for data access.</param>
    /// <param name="logger">The logger to be used for logging information and errors.</param>
    public ClinicRepository(ClinicDbContext context, ILogger<ClinicRepository> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves a list of clinics with optional pagination.
    /// </summary>
    /// <param name="skip">Number of clinics to skip for pagination.</param>
    /// <param name="take">Number of clinics to take for pagination.</param>
    /// <returns>A list of clinics.</returns>
    public async Task<IEnumerable<Clinic?>> GetAllClinicsAsync(int skip = 0, int take = 100)
    {
        try
        {
            // Log the request for getting clinics with pagination
            logger.LogInformation("Getting clinics with skip: {Skip}, take: {Take}", skip, take);
            
            // Fetch clinics from the database with pagination
            var clinics = await context.Clinics
                .Skip(skip) // Skip the specified number of clinics
                .Take(take) // Take the specified number of clinics
                .ToListAsync(); // Convert the result to a list
            
            // Log the number of clinics retrieved
            logger.LogInformation("Retrieved {Count} clinics", clinics.Count());
            
            // Return the list of clinics
            return clinics;
        }
        catch (Exception ex)
        {
            // Log the error if an exception occurs
            logger.LogError(ex, "An error occurred while getting all clinics");
            throw;
        }
    }

    /// <summary>
    /// Retrieves a clinic by its ID.
    /// </summary>
    /// <param name="id">The ID of the clinic to retrieve.</param>
    /// <returns>The clinic with the specified ID.</returns>
    public async Task<Clinic?> GetClinicByIdAsync(long id)
    {
        try
        {
            // Log the request for getting a clinic by ID
            logger.LogInformation("Getting clinic with id: {ClinicId}", id);
            
            // Fetch the clinic from the database by ID
            var clinic = await context.Clinics.FindAsync(id);
            
            if (clinic == null)
            {
                // Log a warning if the clinic is not found
                logger.LogWarning("Clinic with id: {ClinicId} not found", id);
            }
            else
            {
                // Log the retrieved clinic details
                logger.LogInformation("Retrieved clinic with id: {ClinicId}", id);
            }
            
            // Return the clinic details
            return clinic;
        }
        catch (Exception ex)
        {
            // Log the error if an exception occurs
            logger.LogError(ex, "An error occurred while getting clinic with id {ClinicId}", id);
            throw;
        }
    }

    /// <summary>
    /// Adds a new clinic.
    /// </summary>
    /// <param name="clinic">The clinic to add.</param>
    /// <returns>The added clinic.</returns>
    public async Task<Clinic?> AddClinicAsync(Clinic? clinic)
    {
        try
        {
            if (clinic != null)
            {
                // Log the request for adding a new clinic
                logger.LogInformation("Adding new clinic");
                
                // Add the new clinic to the database
                context.Clinics.Add(clinic);
                await context.SaveChangesAsync(); // Save the changes to the database
                
                // Log the ID of the added clinic
                logger.LogInformation("Added clinic with id: {ClinicId}", clinic.ClinicId);
                
                // Return the added clinic details
                return clinic;
            }
            return null;
        }
        catch (Exception ex)
        {
            // Log the error if an exception occurs
            logger.LogError(ex, "An error occurred while adding a new clinic");
            throw;
        }
    }

    /// <summary>
    /// Updates an existing clinic.
    /// </summary>
    /// <param name="clinic">The updated clinic details.</param>
    public async Task UpdateClinicAsync(Clinic clinic)
    {
        try
        {
            // Log the request for updating a clinic
            logger.LogInformation("Updating clinic with id: {ClinicId}", clinic.ClinicId);
            
            // Mark the clinic entity as modified
            context.Entry(clinic).State = EntityState.Modified;
            await context.SaveChangesAsync(); // Save the changes to the database
            
            // Log the ID of the updated clinic
            logger.LogInformation("Updated clinic with id: {ClinicId}", clinic.ClinicId);
        }
        catch (Exception ex)
        {
            // Log the error if an exception occurs
            logger.LogError(ex, "An error occurred while updating clinic with id {ClinicId}", clinic.ClinicId);
            throw;
        }
    }

    /// <summary>
    /// Deletes a clinic by its ID.
    /// </summary>
    /// <param name="id">The ID of the clinic to delete.</param>
    public async Task DeleteClinicAsync(long id)
    {
        try
        {
            // Log the request for deleting a clinic
            logger.LogInformation("Deleting clinic with id: {ClinicId}", id);
            
            // Fetch the clinic from the database by ID
            var clinic = await context.Clinics.FindAsync(id);
            
            if (clinic != null)
            {
                // Remove the clinic from the database
                context.Clinics.Remove(clinic);
                await context.SaveChangesAsync(); // Save the changes to the database
                
                // Log the ID of the deleted clinic
                logger.LogInformation("Deleted clinic with id: {ClinicId}", id);
            }
        }
        catch (Exception ex)
        {
            // Log the error if an exception occurs
            logger.LogError(ex, "An error occurred while deleting clinic with id {ClinicId}", id);
            throw;
        }
    }
}
