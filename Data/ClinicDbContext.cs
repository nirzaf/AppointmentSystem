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
            .HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId);

        // Configures the relationship between Appointment and Doctor
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId);

        // Configures the relationship between Appointment and Clinic
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Clinic)
            .WithMany(c => c.Appointments)
            .HasForeignKey(a => a.ClinicId);
        
        // Configures the relationship between Clinic and Appointments
        modelBuilder.Entity<Clinic>()
            .HasMany(c => c.Appointments)
            .WithOne(a => a.Clinic)
            .HasForeignKey(a => a.ClinicId);

        // Configures the relationship between Patient and Appointments
        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Appointments)
            .WithOne(a => a.Patient)
            .HasForeignKey(a => a.PatientId);

        // Configures the relationship between Doctor and Appointments
        modelBuilder.Entity<Doctor>()
            .HasMany(d => d.Appointments)
            .WithOne(a => a.Doctor)
            .HasForeignKey(a => a.DoctorId);
    }
}
