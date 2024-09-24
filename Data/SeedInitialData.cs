using AppointmentSystem.Models;
using Bogus;

namespace AppointmentSystem.Data;

public static class SeedInitialData
{
    /// <summary>
    /// Initializes the database with initial data if it is empty.
    /// </summary>
    /// <param name="context">The database context to be used for data access.</param>
    public static void Initialize(ClinicDbContext context)
    {
        // Check if the database has already been seeded
        if (context.Clinics.Any() || context.Doctors.Any() || context.Patients.Any() || context.Appointments.Any())
        {
            return; // Database has been seeded
        }

        // Seed Clinics
        var clinics = new Faker<Clinic>()
            .RuleFor(c => c.Name, f => f.Company.CompanyName()) // Generate random company names for clinic names
            .RuleFor(c => c.Address, f => f.Address.FullAddress()) // Generate random full addresses for clinic addresses
            .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber()) // Generate random phone numbers for clinic phone numbers
            .Generate(10); // Generate 10 clinic records

        context.Clinics.AddRange(clinics); // Add the generated clinics to the database context
        context.SaveChanges(); // Save the changes to the database

        // Seed Doctors
        var doctors = new Faker<Doctor>()
            .RuleFor(d => d.FirstName, f => f.Name.FirstName()) // Generate random first names for doctors
            .RuleFor(d => d.LastName, f => f.Name.LastName()) // Generate random last names for doctors
            .RuleFor(d => d.Specialization, f => f.Name.JobTitle()) // Generate random job titles for doctor specializations
            .RuleFor(d => d.PhoneNumber, f => f.Phone.PhoneNumber()) // Generate random phone numbers for doctors
            .RuleFor(d => d.Email, (f, d) => f.Internet.Email(d.FirstName, d.LastName)) // Generate random email addresses for doctors
            .Generate(100); // Generate 100 doctor records

        context.Doctors.AddRange(doctors); // Add the generated doctors to the database context
        context.SaveChanges(); // Save the changes to the database

        // Seed Patients
        var patients = new Faker<Patient>()
            .RuleFor(p => p.FirstName, f => f.Name.FirstName()) // Generate random first names for patients
            .RuleFor(p => p.LastName, f => f.Name.LastName()) // Generate random last names for patients
            .RuleFor(p => p.DateOfBirth, f => f.Date.Past(80)) // Generate random past dates for patient date of birth
            .RuleFor(p => p.Gender, f => f.PickRandom<Gender>()) // Randomly pick a gender for patients
            .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber()) // Generate random phone numbers for patients
            .RuleFor(p => p.Email, (f, p) => f.Internet.Email(p.FirstName, p.LastName)) // Generate random email addresses for patients
            .RuleFor(p => p.Address, f => f.Address.FullAddress()) // Generate random full addresses for patients
            .Generate(20); // Generate 20 patient records

        context.Patients.AddRange(patients); // Add the generated patients to the database context
        context.SaveChanges(); // Save the changes to the database

        // Seed Appointments
        var appointments = new Faker<Appointment>()
            .RuleFor(a => a.PatientId, f => f.PickRandom(patients).PatientId) // Randomly pick a patient ID for appointments
            .RuleFor(a => a.DoctorId, f => f.PickRandom(doctors).DoctorId) // Randomly pick a doctor ID for appointments
            .RuleFor(a => a.ClinicId, f => f.PickRandom(clinics).ClinicId) // Randomly pick a clinic ID for appointments
            .RuleFor(a => a.AppointmentDate, f => f.Date.Future(30)) // Generate random future dates for appointment dates
            .RuleFor(a => a.AppointmentTime, f => new TimeSpan(f.Random.Int(8, 17), 0, 0)) // Generate random times between 8 AM and 5 PM for appointment times
            .RuleFor(a => a.Status, f => f.PickRandom<AppointmentStatus>()) // Randomly pick an appointment status
            .Generate(100); // Generate 100 appointment records

        context.Appointments.AddRange(appointments); // Add the generated appointments to the database context
        context.SaveChanges(); // Save the changes to the database
    }
}
