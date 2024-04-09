using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AppointmentManProject.Controllers;  
using AppointmentManProject.Models;      
using AppointmentManProject.Data;

namespace AppiontmentManProjectUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Create_Works()
        {
            // Arrange
            var controller = new HomeController(); // Assuming HomeController is your controller class
            var dbContext = new ApplicationDbContext(); // Assuming ApplicationDbContext is your DbContext class

            // Create a test appointment
            var appointmentDetails = new Appointment
            {
                appointmentStart = DateTime.Now.AddDays(1), // Set it to be in the future
                appointmentEnd = DateTime.Now.AddDays(2),
                customerPhone = "1234567890", // A valid phone number
                                              // Add other required properties for appointmentDetails
            };

            // Act
            var result = controller.Create(appointmentDetails) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);

            // You can also add additional assertions to check if the appointment is saved in the database, etc.
        }
    }
}
