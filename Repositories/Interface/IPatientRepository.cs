using AppointmentSystem.Models;

namespace AppointmentSystem.Repositories.Interface;

public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAllPatientsAsync(int skip = 0, int take = 100);
    Task<Patient?> GetPatientByIdAsync(long id);
    Task<Patient> AddPatientAsync(Patient patient);
    Task UpdatePatientAsync(Patient patient);
    Task DeletePatientAsync(long id);
}