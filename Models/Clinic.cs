using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Models;

public class Clinic
{
    [Key]
    public long ClinicId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(200)]
    public string? Address { get; set; }

    [Phone]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = null!;
}