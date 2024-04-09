using AppointmentManProject.Data;
using AppointmentManProject.Interfaces;
using AppointmentManProject.Models;

namespace AppointmentManProject.Services
{
    public class YearlyReportGenerator : IReportGenerator
    {
        private readonly ApplicationDbContext _db;

        public YearlyReportGenerator(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Appointment> GenerateReport()
        {
            DateTime today = DateTime.Today;
            DateTime startOfYear = new DateTime(today.Year, 1, 1);
            DateTime startOfNextYear = startOfYear.AddYears(1);

            return _db.appointment
                .Where(a => a.appointmentEnd >= startOfYear && a.appointmentEnd < startOfNextYear)
                .ToList();
        }
    }

}
