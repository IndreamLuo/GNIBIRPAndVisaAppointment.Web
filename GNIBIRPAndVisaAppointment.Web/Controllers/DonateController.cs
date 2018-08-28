using System;
using GNIBIRPAndVisaAppointment.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("Donate")]
    public class DonateController : Controller
    {
        public IActionResult Index(OrderModel model)
        {
            return View(model);
        }
    }
}