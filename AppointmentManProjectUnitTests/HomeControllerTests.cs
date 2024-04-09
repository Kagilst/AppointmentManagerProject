using AppointmentManProject.Controllers;
using AppointmentManProject.Models;      
using AppointmentManProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Routing;
using AppointmentManProject.Interfaces;

namespace AppointmentManProjectUnitTests
{
    [TestClass]
    public class HomeControllerTests
    {
        //Tests to see if inserting correct Data works.
        [TestMethod]
        public void Create_Works()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>(); //Creates Mock Logger to imitate ILogger<HomeController>
            var options = new DbContextOptionsBuilder<ApplicationDbContext>() //Creates in-memory database for testing
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            var reportGeneratorMock = new Mock<IReportGenerator>();

            var controller = new HomeController(loggerMock.Object, dbContext, reportGeneratorMock.Object);


            //Test data
            var appointmentDetails = new Appointment
            {
                appointmentType = "Check-up", //Valid Type
                customerName = "John Smith", //Valid Name
                customerPhone = "1234567890", //Valid Phone
                appointmentStart = DateTime.Now.AddDays(1), //Sets appointment in the future
                appointmentEnd = DateTime.Now.AddDays(2), //Sets to be after appointmentStart

            };

            // Act
            //Calls create method and passes in test data
            var result = controller.Create(appointmentDetails) as RedirectToActionResult; 

            // Assert
            Assert.IsNotNull(result); //Should have a RedirectToActionResult value
            Assert.AreEqual("Index", result.ActionName);//Passes if ActionName redirects to Index
        }
        
        //Testing Past Appointment Validation
        [TestMethod]
        public void Create_PastAppointment_ReturnsViewWithModelError()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>(); //Creates Mock Logger to imitate ILogger<HomeController>
            var options = new DbContextOptionsBuilder<ApplicationDbContext>() //Creates in-memory database for testing
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            var reportGeneratorMock = new Mock<IReportGenerator>();

            var controller = new HomeController(loggerMock.Object, dbContext, reportGeneratorMock.Object);


            //Test data
            var appointmentDetails = new Appointment
            {
                appointmentType = "Check-up", //Valid Type
                customerName = "John Smith", //Valid Name
                customerPhone = "1234567890", //Valid Phone
                appointmentStart = DateTime.Now.AddDays(-1), //Sets Date in the past
                appointmentEnd = DateTime.Now.AddDays(2)
            };

            // Act
            //Calls create method and passes in test data
            var result = controller.Create(appointmentDetails) as ViewResult;

            // Assert
            Assert.IsNotNull(result); //result has a value
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey("appointmentStart")); //checks for appointmentStart
            //Verify that appointmentStart ErrorMessage is correct
            Assert.AreEqual("Appointment cannot be in the past.", result.ViewData.ModelState["appointmentStart"].Errors[0].ErrorMessage); 
        }

        //Testing Start Before End Validation
        [TestMethod]
        public void Create_EndBeforeStart_ReturnsViewWithModelError()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>(); //Creates Mock Logger to imitate ILogger<HomeController>
            var options = new DbContextOptionsBuilder<ApplicationDbContext>() //Creates in-memory database for testing
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            var reportGeneratorMock = new Mock<IReportGenerator>();

            var controller = new HomeController(loggerMock.Object, dbContext, reportGeneratorMock.Object);


            //Test Data
            var appointmentDetails = new Appointment
            {
                appointmentType = "Check-up", //Valid Type
                customerName = "John Smith", //Valid Name
                customerPhone = "1234567890", //Valid Phone
                appointmentStart = DateTime.Now.AddDays(2), // Set it to be in the future
                appointmentEnd = DateTime.Now.AddDays(1) // Set end before start
            };

            // Act
            //Calls create method and passes in test data
            var result = controller.Create(appointmentDetails) as ViewResult;

            // Assert
            Assert.IsNotNull(result); //result has a value
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey("appointmentEnd")); //checks for appointmentEnd
            //Verify that appointmentEnd before appointmentStart ErrorMessage is correct
            Assert.AreEqual("Appointment end must be after start.", result.ViewData.ModelState["appointmentEnd"].Errors[0].ErrorMessage);
        }
    }


}