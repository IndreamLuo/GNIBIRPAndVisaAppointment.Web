using System;
using GNIBIRPAndVisaAppointment.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("Application")]
    public class ApplicationController : Controller
    {
        public IActionResult Index(OrderModel model)
        {
            return View(model);
        }

        // [Route("PlaceOrder")]
        // public IActionResult PlaceOrder()
        // {
        //     return View();
        // }

        // [Route("Pay")]
        // public IActionResult Pay()
        // {
        //     return View();
        // }

        // [Route("ConfirmPayment")]
        // public IActionResult ConfirmPayment()
        // {
        //     return View();
        // }

        // [Route("Status")]
        // public IActionResult Status()
        // {
        //     return View();
        // }
    }
}