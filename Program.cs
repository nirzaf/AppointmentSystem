using Microsoft.EntityFrameworkCore;
using AppointmentSystem.Data;
using AppointmentSystem.Repositories;
using AppointmentSystem.Repositories.Implementation;
using AppointmentSystem.Repositories.Interface;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.SQLite(connectionString, tableName: "Logs")
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "AppointmentSystem")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .Enrich.WithProperty("MachineName", Environment.MachineName)
    .Enrich.WithProperty("UserName", Environment.UserName)
    .Enrich.WithProperty("ProcessId", Environment.ProcessId)
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

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
    options.AddPolicy("AllowAllOrigins", c => { c.AllowAnyOrigin(); });
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
