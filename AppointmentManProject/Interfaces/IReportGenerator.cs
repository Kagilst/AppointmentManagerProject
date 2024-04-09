using AppointmentManProject.Models;

namespace AppointmentManProject.Interfaces
{
    public interface IReportGenerator
    {
        IEnumerable<Appointment> GenerateReport();
    }
}
