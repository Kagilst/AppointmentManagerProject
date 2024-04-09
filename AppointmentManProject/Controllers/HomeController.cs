using AppointmentManProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using AppointmentManProject.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Bcpg;
using AppointmentManProject.Data;
using Microsoft.AspNetCore.Authorization;
using AppointmentManProject.Interfaces;
using AppointmentManProject.Services;

namespace AppointmentManProject.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private  IReportGenerator _reportGenerator;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IReportGenerator reportGenerator)
        {
            _logger = logger;
            _db = db;
            _reportGenerator = reportGenerator;
        }

        //HomePage
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                List<Appointment> appointment = new List<Appointment>();
                appointment = _db.appointment.ToList();
                return View(appointment);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        //logout function
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }

        //Reports Pages
        private IActionResult GenerateReport(string reportType)
        {
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                IEnumerable<Appointment> appointments = null;

                if (reportType == "Monthly")
                {
                    _reportGenerator = new MonthlyReportGenerator(_db);
                    appointments = _reportGenerator.GenerateReport();
                }
                else if (reportType == "Yearly")
                {
                    _reportGenerator = new YearlyReportGenerator(_db);
                    appointments = _reportGenerator.GenerateReport();
                }

                return View(appointments);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public IActionResult MonthlyReport()
        {
            return GenerateReport("Monthly");
        }

        public IActionResult YearlyReport()
        {
            return GenerateReport("Yearly");
        }


        //Code to check whether or not an appointment overlaps with anothers
        private bool CheckForOverlappingAppointments(Appointment newAppointment)
        {
            var existingAppointments = _db.appointment
                .Where(a => a.appointmentId != newAppointment.appointmentId)  // Excludes the current appointment if user is updating existing appointment
                .ToList();

            foreach (var existingAppointment in existingAppointments)
            {
                if (newAppointment.appointmentStart < existingAppointment.appointmentEnd
                    && newAppointment.appointmentEnd > existingAppointment.appointmentStart)
                {
                    return true; // If overlap is found
                }
            }

            return false; // If no overlap is found
        }

        //Create View Code
        public IActionResult Create() 
        {
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }


        [HttpPost]
        public IActionResult Create(Appointment appointmentDetails)
        {

            if (ModelState.IsValid)
            {
                //makes sure appointment is not in the past
                DateTime now = DateTime.Now;

                if (appointmentDetails.appointmentStart < now)
                {
                    ModelState.AddModelError("appointmentStart", "Appointment cannot be in the past.");
                    return View(appointmentDetails);
                }

                //check if appointment end is before start
                if (appointmentDetails.appointmentEnd <= appointmentDetails.appointmentStart)
                {
                    ModelState.AddModelError("appointmentEnd", "Appointment end must be after start.");
                    return View(appointmentDetails);
                }
                
                //formats phone number before saving
                appointmentDetails.customerPhone = string.Format("({0})-{1}-{2}",
                    appointmentDetails.customerPhone.Substring(0, 3),
                    appointmentDetails.customerPhone.Substring(3, 3),
                    appointmentDetails.customerPhone.Substring(6, 4));

                //Checking for overlap with existing appointment
                var overlapping = CheckForOverlappingAppointments(appointmentDetails);

                if (overlapping)
                {
                    ModelState.AddModelError("appointmentStart", "Appointment overlaps with an existing appointment.");
                    return View(appointmentDetails);
                }

                //Passes all validation checks and saves
                _db.appointment.Add(appointmentDetails);
                _db.SaveChanges();

                //return to homepage
                return RedirectToAction("Index");
            }

            //return to create page if fails
            return View();
        }
        //Edit View Code
        public IActionResult Edit(int? Id)
        {
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
            var appointmentDetails = _db.appointment.Find(Id);
            if (appointmentDetails == null)
            {
                return NotFound();
            }

            return View(appointmentDetails);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }

        [HttpPost]
        public IActionResult Edit(Appointment appointmentDetails)
        {

            if (ModelState.IsValid)
            {
                //makes sure appointment is not in the past
                DateTime now = DateTime.Now;

                if (appointmentDetails.appointmentStart < now)
                {
                    ModelState.AddModelError("appointmentStart", "Appointment cannot be in the past.");
                    return View(appointmentDetails);
                }
                
                //check if appointment end is before start
                if (appointmentDetails.appointmentEnd <= appointmentDetails.appointmentStart)
                {
                    ModelState.AddModelError("appointmentEnd", "Appointment end must be after start.");
                    return View(appointmentDetails);
                }

                //formats phone number before saving
                appointmentDetails.customerPhone = string.Format("({0})-{1}-{2}",
                    appointmentDetails.customerPhone.Substring(0, 3),
                    appointmentDetails.customerPhone.Substring(3, 3),
                    appointmentDetails.customerPhone.Substring(6, 4));
                
                //Checking for overlap with existing appointment
                var overlapping = CheckForOverlappingAppointments(appointmentDetails);

                if (overlapping)
                {
                    ModelState.AddModelError("appointmentStart", "Appointment overlaps with an existing appointment.");
                    return View(appointmentDetails);
                }

                //Passes all validation checks and saves
                _db.appointment.Update(appointmentDetails);
                _db.SaveChanges();

                //return to homepage
                return RedirectToAction("Index");
            }

            //return to create page if fails
            return View();
        }

        //Delete appointment code
        public IActionResult Delete(int? Id)
        {
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {

                var appointmentDetails = _db.appointment.Find(Id);
            if (appointmentDetails == null)
            {
                return NotFound();
            }

            return View(appointmentDetails);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }

        [HttpPost]
        public IActionResult DeleteAppointment(Appointment appointmentDetails)
        {
            
            if (appointmentDetails == null)
            {
                return NotFound();
            }

            _db.appointment.Remove(appointmentDetails);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}