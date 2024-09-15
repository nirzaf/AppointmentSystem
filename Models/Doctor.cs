using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Specialization { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
