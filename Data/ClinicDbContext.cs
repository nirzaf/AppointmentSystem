using AppointmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Data;

public class ClinicDbContext(DbContextOptions<ClinicDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Represents the Clinics table in the database.
    /// </summary>
    public DbSet<Clinic> Clinics { get; set; }

    /// <summary>
    /// Represents the Doctors table in the database.
    /// </summary>
    public DbSet<Doctor> Doctors { get; set; }

    /// <summary>
    /// Represents the Patients table in the database.
    /// </summary>
    public DbSet<Patient> Patients { get; set; }

    /// <summary>
    /// Represents the Appointments table in the database.
    /// </summary>
    public DbSet<Appointment> Appointments { get; set; }

    /// <summary>
    /// Configures the relationships between the entities.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure the relationships.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configures the relationship between Appointment and Patient
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient) // Each appointment has one patient
            .WithMany(p => p.Appointments) // Each patient can have many appointments
            .HasForeignKey(a => a.PatientId); // Foreign key in Appointment table

        // Configures the relationship between Appointment and Doctor
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor) // Each appointment has one doctor
            .WithMany(d => d.Appointments) // Each doctor can have many appointments
            .HasForeignKey(a => a.DoctorId); // Foreign key in Appointment table

        // Configures the relationship between Appointment and Clinic
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Clinic) // Each appointment has one clinic
            .WithMany(c => c.Appointments) // Each clinic can have many appointments
            .HasForeignKey(a => a.ClinicId); // Foreign key in Appointment table
        
        // Configures the relationship between Clinic and Appointments
        modelBuilder.Entity<Clinic>()
            .HasMany(c => c.Appointments) // Each clinic can have many appointments
            .WithOne(a => a.Clinic) // Each appointment has one clinic
            .HasForeignKey(a => a.ClinicId); // Foreign key in Appointment table

        // Configures the relationship between Patient and Appointments
        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Appointments) // Each patient can have many appointments
            .WithOne(a => a.Patient) // Each appointment has one patient
            .HasForeignKey(a => a.PatientId); // Foreign key in Appointment table

        // Configures the relationship between Doctor and Appointments
        modelBuilder.Entity<Doctor>()
            .HasMany(d => d.Appointments) // Each doctor can have many appointments
            .WithOne(a => a.Doctor) // Each appointment has one doctor
            .HasForeignKey(a => a.DoctorId); // Foreign key in Appointment table
    }
}
