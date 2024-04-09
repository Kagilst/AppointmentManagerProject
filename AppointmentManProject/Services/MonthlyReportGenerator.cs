using AppointmentManProject.Data;
using AppointmentManProject.Interfaces;
using AppointmentManProject.Models;

namespace AppointmentManProject.Services
{
    public class MonthlyReportGenerator : IReportGenerator
    {
        private readonly ApplicationDbContext _db;

        public MonthlyReportGenerator(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Appointment> GenerateReport()
        {
            DateTime today = DateTime.Today;
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime startOfNextMonth = startOfMonth.AddMonths(1);

            return _db.appointment
                .Where(a => a.appointmentStart >= startOfMonth && a.appointmentStart < startOfNextMonth)
                .ToList();
        }


    }
}
