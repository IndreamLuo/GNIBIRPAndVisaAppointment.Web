using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    public class AppointmentPredictionViewComponent : ViewComponent
    {
        IDomainHub DomainHub;

        public AppointmentPredictionViewComponent(IDomainHub domainHub)
        {
            DomainHub = domainHub;
        }

        public IViewComponentResult Invoke(bool? isNew = null)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            ViewBag.Assignments = applicationManager.CachedAssignments.ContainsKey(AssignmentStatus.Accepted)
                ? applicationManager.CachedAssignments[AssignmentStatus.Accepted]
                : applicationManager.GetAssignments(AssignmentStatus.Accepted);
                
            return View();
        }
    }
}