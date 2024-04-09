using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class AddAppointmentTests
    {
        HomeController homeControler;

        [Test]
        public void Create_Works()
        {
            // Arrange
            var controller = new HomeController(); 
            var dbContext = new ApplicationDbContext(); 

            // Create a test appointment
            var appointmentDetails = new Appointment
            {
                appointmentStart = DateTime.Now.AddDays(1), 
                appointmentEnd = DateTime.Now.AddDays(2),
                customerPhone = "1234567890", 
                                              
            };

            // Act
            var result = controller.Create(appointmentDetails) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }

    }
}
