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
    private readonly ClinicDbContext context;
    private readonly ILogger<DoctorRepository> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DoctorRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to be used for data access.</param>
    /// <param name="logger">The logger to be used for logging information and errors.</param>
    public DoctorRepository(ClinicDbContext context, ILogger<DoctorRepository> logger)
    {
        this.context = context;
        this.logger = logger;
    }

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
            logger.LogInformation("Getting doctors with skip: {Skip} and take: {Take}", skip, take);
            var doctors = await context.Doctors
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            logger.LogInformation("Retrieved {Count} doctors", doctors.Count());
            return doctors;
        }
        catch (Exception ex)
        {
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
            logger.LogInformation("Getting doctor with id: {DoctorId}", id);
            var doctor = await context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                logger.LogWarning("Doctor with id: {DoctorId} not found", id);
            }
            else
            {
                logger.LogInformation("Retrieved doctor with id: {DoctorId}", id);
            }
            return doctor;
        }
        catch (Exception ex)
        {
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
            logger.LogInformation("Adding new doctor");
            context.Doctors.Add(doctor);
            await context.SaveChangesAsync();
            logger.LogInformation("Added doctor with id: {DoctorId}", doctor.DoctorId);
            return doctor;
        }
        catch (Exception ex)
        {
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
            logger.LogInformation("Updating doctor with id: {DoctorId}", doctor.DoctorId);
            context.Entry(doctor).State = EntityState.Modified;
            await context.SaveChangesAsync();
            logger.LogInformation("Updated doctor with id: {DoctorId}", doctor.DoctorId);
        }
        catch (Exception ex)
        {
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
            logger.LogInformation("Deleting doctor with id: {DoctorId}", id);
            var doctor = await context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                context.Doctors.Remove(doctor);
                await context.SaveChangesAsync();
                logger.LogInformation("Deleted doctor with id: {DoctorId}", id);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting doctor with id {DoctorId}", id);
            throw;
        }
    }
}
