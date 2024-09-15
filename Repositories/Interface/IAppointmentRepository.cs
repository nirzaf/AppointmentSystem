using AppointmentSystem.Models;

namespace AppointmentSystem.Repositories.Interface;

public interface IAppointmentRepository
{
    Task<IEnumerable<Appointment>?> GetAllAppointmentsAsync();
    Task<Appointment?> GetAppointmentByIdAsync(int id);
    Task<Appointment?> CreateAppointmentAsync(Appointment appointment);
    Task<Appointment?> UpdateAppointmentAsync(Appointment appointment);
    Task DeleteAppointmentAsync(int id);
}