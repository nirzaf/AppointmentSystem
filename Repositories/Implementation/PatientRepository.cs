using AppointmentSystem.Data;
using AppointmentSystem.Models;
using AppointmentSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Repositories.Implementation;

/// <summary>
/// Repository class for managing patients in the database.
/// </summary>
public class PatientRepository(ClinicDbContext context, ILogger<PatientRepository> logger) : IPatientRepository
{
    private readonly ClinicDbContext context;
    private readonly ILogger<PatientRepository> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to be used for data access.</param>
    /// <param name="logger">The logger to be used for logging information and errors.</param>
    public PatientRepository(ClinicDbContext context, ILogger<PatientRepository> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves a list of patients with optional pagination.
    /// </summary>
    /// <param name="skip">Number of patients to skip for pagination.</param>
    /// <param name="take">Number of patients to take for pagination.</param>
    /// <returns>A list of patients.</returns>
    public async Task<IEnumerable<Patient>> GetAllPatientsAsync(int skip = 0, int take = 100)
    {
        try
        {
            logger.LogInformation("Getting patients with skip: {Skip}, take: {Take}", skip, take);
            var patients = await context.Patients
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            logger.LogInformation("Retrieved {Count} patients", patients.Count());
            return patients;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting all patients");
            throw;
        }
    }

    /// <summary>
    /// Retrieves a patient by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient to retrieve.</param>
    /// <returns>The patient with the specified ID.</returns>
    public async Task<Patient?> GetPatientByIdAsync(long id)
    {
        try
        {
            logger.LogInformation("Getting patient with id: {PatientId}", id);
            var patient = await context.Patients.FindAsync(id);
            if (patient == null)
            {
                logger.LogWarning("Patient with id: {PatientId} not found", id);
            }
            else
            {
                logger.LogInformation("Retrieved patient with id: {PatientId}", id);
            }
            return patient;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting patient with id {PatientId}", id);
            throw;
        }
    }

    /// <summary>
    /// Adds a new patient.
    /// </summary>
    /// <param name="patient">The patient to add.</param>
    /// <returns>The added patient.</returns>
    public async Task<Patient> AddPatientAsync(Patient patient)
    {
        try
        {
            logger.LogInformation("Adding new patient");
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
            logger.LogInformation("Added patient with id: {PatientId}", patient.PatientId);
            return patient;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while adding a new patient");
            throw;
        }
    }

    /// <summary>
    /// Updates an existing patient.
    /// </summary>
    /// <param name="patient">The updated patient details.</param>
    public async Task UpdatePatientAsync(Patient patient)
    {
        try
        {
            logger.LogInformation("Updating patient with id: {PatientId}", patient.PatientId);
            context.Entry(patient).State = EntityState.Modified;
            await context.SaveChangesAsync();
            logger.LogInformation("Updated patient with id: {PatientId}", patient.PatientId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating patient with id {PatientId}", patient.PatientId);
            throw;
        }
    }

    /// <summary>
    /// Deletes a patient by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient to delete.</param>
    public async Task DeletePatientAsync(long id)
    {
        try
        {
            logger.LogInformation("Deleting patient with id: {PatientId}", id);
            var patient = await context.Patients.FindAsync(id);
            if (patient != null)
            {
                context.Patients.Remove(patient);
                await context.SaveChangesAsync();
                logger.LogInformation("Deleted patient with id: {PatientId}", id);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting patient with id {PatientId}", id);
            throw;
        }
    }
}