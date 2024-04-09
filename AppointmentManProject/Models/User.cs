using System.ComponentModel.DataAnnotations.Schema;
using AppointmentManProject.Data;

namespace AppointmentManProject.Models
{
    [Table("user")]
    public class User
    {
        private ApplicationDbContext context;

        public int userId { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
    }
}
