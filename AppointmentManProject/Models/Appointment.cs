using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppointmentManProject.Data;

namespace AppointmentManProject.Models
{
    [Table("appointment")]
    public class Appointment
    {
        private ApplicationDbContext context;

        [Key]
        public int appointmentId { get; set; }
        public string appointmentType { get; set; }
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Please enter a valid name with only letters.")]
        public string customerName { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a 10-digit phone number.")]
        public string customerPhone { get; set; }
        public DateTime appointmentStart { get; set; }
        public DateTime appointmentEnd
        {
            get; set;

        }
    }
}
