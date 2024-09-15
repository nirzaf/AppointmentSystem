using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Models
{
    public class Clinic
    {
        public int ClinicId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
