using AppointmentSystem.Models;

namespace AppointmentSystem.Repositories.Interface;

public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAllPatientsAsync();
    Task<Patient?> GetPatientByIdAsync(long id);
    Task<Patient> AddPatientAsync(Patient patient);
    Task UpdatePatientAsync(Patient patient);
    Task DeletePatientAsync(long id);
}