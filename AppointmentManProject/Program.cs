using AppointmentManProject.Models;
using Microsoft.EntityFrameworkCore;
using AppointmentManProject.Data;
using AppointmentManProject.UserRepository;
using AppointmentManProject.Services;
using AppointmentManProject.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//services
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReportGenerator, MonthlyReportGenerator>(); 
builder.Services.AddScoped<IReportGenerator, YearlyReportGenerator>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

//timer that user stays authenticated
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); //
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
