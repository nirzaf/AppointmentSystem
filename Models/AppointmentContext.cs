using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Models;

public class AppointmentContext : DbContext
{ 
    public AppointmentContext(DbContextOptions<AppointmentContext> options) : base(options)
    {
    }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Clinic> Clinics { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>().ToTable("Appointments");
        modelBuilder.Entity<Patient>().ToTable("Patients");
        modelBuilder.Entity<Doctor>().ToTable("Doctors");
        modelBuilder.Entity<Clinic>().ToTable("Clinics");
    }
}