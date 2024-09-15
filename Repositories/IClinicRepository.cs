using AppointmentSystem.Models;

namespace AppointmentSystem.Repositories
{
    public interface IClinicRepository
    {
        Task<IEnumerable<Clinic>> GetAllClinicsAsync();
        Task<Clinic> GetClinicByIdAsync(int id);
        Task<Clinic> AddClinicAsync(Clinic clinic);
        Task UpdateClinicAsync(Clinic clinic);
        Task DeleteClinicAsync(int id);
    }
}
