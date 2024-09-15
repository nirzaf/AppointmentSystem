using AppointmentSystem.Models;

namespace AppointmentSystem.Repositories.Interface;

public interface IDoctorRepository
{
    Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
    Task<Doctor> GetDoctorByIdAsync(int id);
    Task<Doctor> AddDoctorAsync(Doctor doctor);
    Task UpdateDoctorAsync(Doctor doctor);
    Task DeleteDoctorAsync(int id);
}