using System;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("Api")]
    public class ApiController : Controller
    {
        [HttpPost]
        [Route("Application/Accepted")]
        public IActionResult ApplicationAccepted(string token)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("Application/Appoint")]
        public IActionResult ApplicationAppoint(string token, string id)
        {
            throw new NotImplementedException();
        }
    }
}