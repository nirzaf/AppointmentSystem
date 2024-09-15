using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
