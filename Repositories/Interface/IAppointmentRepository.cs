using AppointmentSystem.Models;

namespace AppointmentSystem.Repositories.Interface;

public interface IAppointmentRepository
{
    Task<IEnumerable<Appointment?>> GetAllAppointmentsAsync();
    Task<Appointment?> GetAppointmentByIdAsync(long id);
    Task<Appointment?> CreateAppointmentAsync(Appointment? appointment);
    Task<Appointment?> UpdateAppointmentAsync(Appointment appointment);
    Task DeleteAppointmentAsync(long id);
}