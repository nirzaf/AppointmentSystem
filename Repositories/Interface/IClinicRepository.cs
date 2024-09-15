using AppointmentSystem.Models;

namespace AppointmentSystem.Repositories.Interface;

public interface IClinicRepository
{
    Task<IEnumerable<Clinic?>> GetAllClinicsAsync();
    Task<Clinic?> GetClinicByIdAsync(long id);
    Task<Clinic?> AddClinicAsync(Clinic? clinic);
    Task UpdateClinicAsync(Clinic clinic);
    Task DeleteClinicAsync(long id);
}