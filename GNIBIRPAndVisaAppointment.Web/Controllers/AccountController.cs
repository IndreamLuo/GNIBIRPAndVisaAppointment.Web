using System;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public IActionResult Login(string id, string password)
        {
            return View();
        }
    }
}