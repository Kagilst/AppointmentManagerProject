using AppointmentManProject.Models;
using Microsoft.AspNetCore.Mvc;
using AppointmentManProject.Data;
using AppointmentManProject.UserRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace AppointmentManProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IUserRepository _userRepository;

        public LoginController(ILogger<HomeController> logger, ApplicationDbContext db, IUserRepository userRepository)
        {
            _logger = logger;
            _db = db;
            _userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);

            //make sure username and password is case sensitive
            if (user != null &&
                string.Equals(user.userName, username, StringComparison.Ordinal) && 
                string.Equals(user.userPassword, password, StringComparison.Ordinal))
            {
                HttpContext.Session.SetString("IsLoggedIn", "true");
                HttpContext.Session.SetString("Username", user.userName);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // If login failed, display an error message
                ViewBag.ErrorMessage = "Incorrect username or password.";
                return View();
            }
        }


    }
}
