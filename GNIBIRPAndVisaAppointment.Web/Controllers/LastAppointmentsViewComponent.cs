using System.Linq;
using System.Threading.Tasks;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Appointment;
using GNIBIRPAndVisaAppointment.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    public class LastAppointmentsViewComponent : ViewComponent
    {
        IDomainHub DomainHub;

        public LastAppointmentsViewComponent(IDomainHub domainHub)
        {
            DomainHub = domainHub;
        }

        public IViewComponentResult Invoke()
        {
            var appointmentManager = DomainHub.GetDomain<IAppointmentManager>();
            var lastAppointments = appointmentManager.GetLastAppointments();
            var models = lastAppointments.Select(appointment => new AppointmentModel(appointment));
            return View(models);
        }
    }
}