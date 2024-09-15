using AppointmentSystem.Models;

namespace AppointmentSystem.Repositories.Interface;

public interface IAppointmentRepository
{
    Task<IEnumerable<Appointment?>> GetAllAppointmentsAsync(int skip = 0, int take = 100);
    Task<Appointment?> GetAppointmentByIdAsync(long id);
    Task<Appointment?> CreateAppointmentAsync(Appointment? appointment);
    Task<Appointment?> UpdateAppointmentAsync(Appointment appointment);
    Task DeleteAppointmentAsync(long id);
}