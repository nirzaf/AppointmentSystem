using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Models;

public class Patient
{
    public Patient()
    {
        Appointments = new List<Appointment>();
    }

    [Key]
    public long PatientId { get; set; }

    [StringLength(50)]
    public required string FirstName { get; set; }

    [StringLength(50)]
    public required string LastName { get; set; }

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [Phone]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    public ICollection<Appointment> Appointments { get; set; }
}

public enum Gender
{
    Male,
    Female,
    Other
}

