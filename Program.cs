global using ILogger = Serilog.ILogger;
using Microsoft.EntityFrameworkCore;
using AppointmentSystem.Data;
using AppointmentSystem.Repositories;
using AppointmentSystem.Repositories.Interface;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.SQLite(
        connectionString,
        tableName: "Logs",
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddSingleton(Log.Logger);

// Add services to the container.
builder.Services.AddDbContext<ClinicDbContext>(options =>
    options.UseSqlite(connectionString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddScoped<IClinicRepository, ClinicRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder => { builder.AllowAnyOrigin(); });
});

var app = builder.Build();

// Initialize seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ClinicDbContext>();
    context.Database.Migrate();
    SeedInitialData.Initialize(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.MapControllers();

app.Run();

// Ensure to flush and close the log when the application shuts down
Log.CloseAndFlush();


