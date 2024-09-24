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
            logger.LogInformation("Getting clinics with skip: {Skip}, take: {Take}", skip, take);
            var clinics = await context.Clinics
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            logger.LogInformation("Retrieved {Count} clinics", clinics.Count());
            return clinics;
        }
        catch (Exception ex)
        {
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
            logger.LogInformation("Getting clinic with id: {ClinicId}", id);
            var clinic = await context.Clinics.FindAsync(id);
            if (clinic == null)
            {
                logger.LogWarning("Clinic with id: {ClinicId} not found", id);
            }
            else
            {
                logger.LogInformation("Retrieved clinic with id: {ClinicId}", id);
            }
            return clinic;
        }
        catch (Exception ex)
        {
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
                logger.LogInformation("Adding new clinic");
                context.Clinics.Add(clinic);
                await context.SaveChangesAsync();
                logger.LogInformation("Added clinic with id: {ClinicId}", clinic.ClinicId);
                return clinic;
            }
            return null;
        }
        catch (Exception ex)
        {
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
            logger.LogInformation("Updating clinic with id: {ClinicId}", clinic.ClinicId);
            context.Entry(clinic).State = EntityState.Modified;
            await context.SaveChangesAsync();
            logger.LogInformation("Updated clinic with id: {ClinicId}", clinic.ClinicId);
        }
        catch (Exception ex)
        {
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
            logger.LogInformation("Deleting clinic with id: {ClinicId}", id);
            var clinic = await context.Clinics.FindAsync(id);
            if (clinic != null)
            {
                context.Clinics.Remove(clinic);
                await context.SaveChangesAsync();
                logger.LogInformation("Deleted clinic with id: {ClinicId}", id);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting clinic with id {ClinicId}", id);
            throw;
        }
    }
}