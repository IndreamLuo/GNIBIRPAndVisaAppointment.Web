using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.AppointmnetLetter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Authorize(Roles="Admin,Manager")]
    [Route("Admin/AppointmentLetter")]
    public partial class AdminAppointmentLetterController : Controller
    {
        readonly IDomainHub DomainHub;

        public AdminAppointmentLetterController(IDomainHub domainHub)
        {
            DomainHub = domainHub;
        }

        public IActionResult Index()
        {
            var appointmentLetterManager = DomainHub.GetDomain<IAppointmentLetterManager>();
            var appointmentLetters = appointmentLetterManager.UnassignedLetters;

            ViewBag.AppointmentLetters = appointmentLetters;
            
            return View();
        }
    }
}