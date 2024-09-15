using AppointmentSystem.Data;
using AppointmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ClinicDbContext _context;

        public AppointmentRepository(ClinicDbContext context)
        {
            _context = context;
        }

        // Implement methods similar to ClinicRepository
    }

    public interface IAppointmentRepository
    {
    }
}
