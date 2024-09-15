using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Models;

public class Doctor
{
    [Key]
    public long DoctorId { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(50)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Specialization { get; set; } = null!;

    [Phone]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = null!;
}