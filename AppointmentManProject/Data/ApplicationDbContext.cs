using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using AppointmentManProject.Models;

namespace AppointmentManProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Appointment> appointment { get; set; }
        public DbSet<User> user { get; set; }
    }
}
