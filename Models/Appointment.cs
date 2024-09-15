using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Models;

public class Appointment
{
    [Key]
    public long AppointmentId { get; set; }

    [Required]
    public long PatientId { get; set; }
    public Patient? Patient { get; set; }

    [Required]
    public long DoctorId { get; set; }
    public required Doctor Doctor { get; set; }

    [Required]
    public long ClinicId { get; set; }
    public required Clinic Clinic { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime AppointmentDate { get; set; }

    [Required]
    [DataType(DataType.Time)]
    public TimeSpan AppointmentTime { get; set; }

    [Required]
    [EnumDataType(typeof(AppointmentStatus))]
    public AppointmentStatus Status { get; set; }

    public string? Notes { get; set; }
}

public enum AppointmentStatus
{
    Scheduled,
    Confirmed,
    Cancelled,
    Completed
}