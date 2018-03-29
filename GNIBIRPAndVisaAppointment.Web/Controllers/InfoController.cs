using System;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("Info")]
    public class InfoController : Controller
    {
        [Route("About/{name}")]
        public IActionResult About(string name)
        {
            throw new NotImplementedException();
        }
    }
}