using Microsoft.EntityFrameworkCore;
using PruebaHospital.Data;
using PruebaHospital.Repositories;
using PruebaHospital.Services;
using PruebaHospital.Validators;
using PruebaHospital.Utils;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<PatientValidator>();
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();


builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<AppointmentService>();

builder.Services.AddScoped<PatientValidator>();
builder.Services.AddScoped<DoctorValidator>();
builder.Services.AddScoped<AppointmentValidator>();

builder.Services.AddScoped<EmailHelper>();

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
    logging.SetMinimumLevel(LogLevel.Information);
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();