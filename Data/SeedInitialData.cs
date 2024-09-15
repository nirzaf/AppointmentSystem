using AppointmentSystem.Models;
using Bogus;

namespace AppointmentSystem.Data;

public static class SeedInitialData
{
    public static void Initialize(ClinicDbContext context)
    {
        if (context.Clinics.Any() || context.Doctors.Any() || context.Patients.Any() || context.Appointments.Any())
        {
            return; // Database has been seeded
        }

        // Seed Clinics
        var clinics = new Faker<Clinic>()
            .RuleFor(c => c.Name, f => f.Company.CompanyName())
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
            .Generate(10);

        context.Clinics.AddRange(clinics);
        context.SaveChanges();

        // Seed Doctors
        var doctors = new Faker<Doctor>()
            .RuleFor(d => d.FirstName, f => f.Name.FirstName())
            .RuleFor(d => d.LastName, f => f.Name.LastName())
            .RuleFor(d => d.Specialization, f => f.Name.JobTitle())
            .RuleFor(d => d.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(d => d.Email, (f, d) => f.Internet.Email(d.FirstName, d.LastName))
            .Generate(100);

        context.Doctors.AddRange(doctors);
        context.SaveChanges();

        // Seed Patients
        var patients = new Faker<Patient>()
            .RuleFor(p => p.FirstName, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(p => p.DateOfBirth, f => f.Date.Past(80))
            .RuleFor(p => p.Gender, f => f.PickRandom<Gender>())
            .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(p => p.Email, (f, p) => f.Internet.Email(p.FirstName, p.LastName))
            .RuleFor(p => p.Address, f => f.Address.FullAddress())
            .Generate(20);

        context.Patients.AddRange(patients);
        context.SaveChanges();

        // Seed Appointments
        var appointments = new Faker<Appointment>()
            .RuleFor(a => a.PatientId, f => f.PickRandom(patients).PatientId)
            .RuleFor(a => a.DoctorId, f => f.PickRandom(doctors).DoctorId)
            .RuleFor(a => a.ClinicId, f => f.PickRandom(clinics).ClinicId)
            .RuleFor(a => a.AppointmentDate, f => f.Date.Future(30))
            .RuleFor(a => a.AppointmentTime, f => new TimeSpan(f.Random.Int(8, 17), 0, 0))
            .RuleFor(a => a.Status, f => f.PickRandom<AppointmentStatus>())
            .Generate(100);

        context.Appointments.AddRange(appointments);
        context.SaveChanges();
    }
}