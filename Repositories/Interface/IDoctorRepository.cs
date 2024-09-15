using AppointmentSystem.Models;

namespace AppointmentSystem.Repositories.Interface;

public interface IDoctorRepository
{
    Task<IEnumerable<Doctor>> GetAllDoctorsAsync(int skip = 0, int take = int.MaxValue);
    Task<Doctor?> GetDoctorByIdAsync(long id);
    Task<Doctor> AddDoctorAsync(Doctor doctor);
    Task UpdateDoctorAsync(Doctor doctor);
    Task DeleteDoctorAsync(long id);
}