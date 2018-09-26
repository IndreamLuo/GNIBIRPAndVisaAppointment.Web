using GNIBIRPAndVisaAppointment.Web.Business;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        IDomainHub DomainHub;
        public UserController(IDomainHub domainHub)
        {
            DomainHub = domainHub;
        }

        public IActionResult Login(string id, string password)
        {
            return View();
        }
    }
}