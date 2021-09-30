using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GNIBIRPAndVisaAppointment.Web.Models;
using Microsoft.Extensions.Configuration;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route(".well-known/pki-validation/godaddy.html")]
        public IActionResult SslCert()
        {
            var cert = this.configuration["AppServiceCert"];
            return Content(cert);
        }
    }
}
